using System.Globalization;
using System.Net.Http;
using Amazon.S3;
using Amazon.S3.Model;
using Aspose.Cells;
using FinancialPlanning.Common;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace FinancialPlanning.Service.Services;

public class FileService(IAmazonS3 s3Client, IConfiguration configuration, HttpClient httpClient)
{
    

    public async Task<string> GetFileAsync(string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = configuration["AWS:BucketName"],
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        return await s3Client.GetPreSignedURLAsync(request);
    }

    /*
     * documentType:
     * {
     *  plan:   0
     *  report: 1
     * }
     */
    public async Task UploadPlanAsync(string key, Stream fileStream)
    {
        var request = new PutObjectRequest
        {
            BucketName = configuration["AWS:BucketName"],
            Key = key,
            InputStream = fileStream,
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        await s3Client.PutObjectAsync(request);
    }
    /*
     * documentType:
     * {
     *  plan:   0
     *  report: 1
     * }
     */
    public bool ValidateFile(FileStream fileStream, byte documentType)
    {
        Console.WriteLine(fileStream.Name);
        string[] validExtension = [".xls", ".xlsx", ".csv"];

        //check file is not empty, not bigger than 500MB and has valid extension
        if (fileStream.Length == 0 || fileStream.Length > Constants.MaxFileSize ||
            !validExtension.Contains(Path.GetExtension(fileStream.Name).ToLower()))
        {
            return false;
        }
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var package = new ExcelPackage(fileStream);
        package = RemoveEmptyRow(package);

        var worksheet = package.Workbook.Worksheets[0];
        var numOfRows = worksheet.Dimension?.Rows ?? 0;

        //check number of column is sufficient
        var numOfColumns = worksheet.Dimension?.Columns ?? 0;
        if (numOfColumns != Constants.TemplateHeader[documentType].Length)
            return false;

        //remove empty row (all cell in row is empty)

        //check file has data
        if (numOfRows < 3)
            return false;

        //check cell content is not null and has valid format
        for (var i = 1; i <= numOfColumns; i++)
        {
            if (!(worksheet.Cells[2, i].Value?.ToString() ?? "").Equals(Constants.TemplateHeader[documentType][i - 1]))
                return false;

            for (var j = 3; j <= numOfRows; j++)
            {
                var cellContext = worksheet.Cells[j, i].Value?.ToString()?.Trim();
                if ((documentType == 0 ? i != 11 && i != 15 : i != 12) && cellContext.IsNullOrEmpty())
                    return false;

                switch (i)
                {
                    case 1:
                        if (!DateTime.TryParseExact(cellContext!, ["dd/MM/yyyy", "dd/MM/yyyy hh:mm:ss tt"], null,
                                DateTimeStyles.None, out _))
                            return false;
                        break;
                    case 6:
                    case 7:
                    case 8 when documentType != 0:
                    case 10 when documentType == 0:
                        if (!decimal.TryParse(cellContext!, NumberStyles.Currency, null, out _))
                            return false;
                        break;
                }
            }
        }

        //check TotalAmount = UnitPrice * Amount
        for (var i = 3; i < numOfRows; i++)
        {
            var unitPrice = decimal.Parse(worksheet.Cells[i, 6].Value.ToString()!);
            var amount = decimal.Parse(worksheet.Cells[i, 7].Value.ToString()!);
            var totalAmount = decimal.Parse(worksheet.Cells[i, documentType != 0 ? 8 : 10].Value.ToString()!);
            if (totalAmount != unitPrice * amount)
                return false;
        }

        return true;
    }

    /*
     * documentType:
     * {
     *  plan:   0
     *  report: 1
     * }
     */
    public List<Expense> ConvertExcelToList(FileStream fileStream, byte documentType)
    {
        var expenses = new List<Expense>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage(fileStream);
        package = RemoveEmptyRow(package);

        var worksheet = package.Workbook.Worksheets[0];
        for (var i = 3; i <= worksheet.Dimension.Rows; i++)
        {
            expenses.Add(new Expense
            {
                No = i - 2,
                Date = DateTime.Parse(worksheet.Cells[i, 1].Value.ToString()!.Trim()),
                Term = worksheet.Cells[i, 2].Value.ToString()!.Trim(),
                Department = worksheet.Cells[i, 3].Value.ToString()!.Trim(),
                ExpenseName = worksheet.Cells[i, 4].Value.ToString()!.Trim(),
                CostType = worksheet.Cells[i, 5].Value.ToString()!.Trim(),
                UnitPrice = decimal.Parse(worksheet.Cells[i, 6].Value.ToString()!.Trim()),
                Amount = decimal.Parse(worksheet.Cells[i, 7].Value.ToString()!.Trim()),
                Currency = documentType == 0 ? worksheet.Cells[i, 8].Value.ToString()!.Trim() : null,
                ExchangeRate = documentType == 0
                    ? double.Parse(worksheet.Cells[i, 9].Value.ToString()!.Trim(), CultureInfo.InvariantCulture)
                    : null,
                TotalAmount = decimal.Parse(worksheet.Cells[i, 10 - documentType * 2].Value.ToString()!.Trim()),
                ProjectName = worksheet.Cells[i, 12 - documentType * 3].Value.ToString()!.Trim(),
                SupplierName = worksheet.Cells[i, 13 - documentType * 3].Value.ToString()!.Trim(),
                PIC = worksheet.Cells[i, 14 - documentType * 3].Value.ToString()!.Trim(),
                Note = worksheet.Cells[i, 15 - documentType * 3].Value?.ToString()?.Trim()
            });
        }

        return expenses;
    }

    public async Task<Stream> ConvertListToExcelAsync(IEnumerable<Expense> expenses, byte documentType)
    {
        //Write list of expenses to ExcelPackage
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package =
            new ExcelPackage(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), Constants.TemplatePath[documentType])));
        var worksheet = package.Workbook.Worksheets[0];
        foreach (var (expense, index) in expenses.Select((value, i) => (value, i)))
        {
            worksheet.Cells[index + 2, 1].Value = expense.Date;
            worksheet.Cells[index + 2, 2].Value = expense.Term;
            worksheet.Cells[index + 2, 3].Value = expense.Department;
            worksheet.Cells[index + 2, 4].Value = expense.ExpenseName;
            worksheet.Cells[index + 2, 5].Value = expense.CostType;
            worksheet.Cells[index + 2, 6].Value = expense.UnitPrice;
            worksheet.Cells[index + 2, 7].Value = expense.Amount;

            //if document is plan
            if (documentType == 0)
            {
                worksheet.Cells[index + 2, 8].Value = expense.Currency;
                worksheet.Cells[index + 2, 9].Value = expense.ExchangeRate;
            }

            worksheet.Cells[index + 2, 10 - documentType * 2].Value = expense.TotalAmount;
            worksheet.Cells[index + 2, 12 - documentType * 3].Value = expense.ProjectName;
            worksheet.Cells[index + 2, 13 - documentType * 3].Value = expense.SupplierName;
            worksheet.Cells[index + 2, 14 - documentType * 3].Value = expense.PIC;
            worksheet.Cells[index + 2, 15 - documentType * 3].Value = expense.Note;
        }

        //Convert ExcelPackage to Stream
        var memoryStream = new MemoryStream();
        await package.SaveAsAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    private ExcelPackage RemoveEmptyRow(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets[0];

        for (var i = 1; i <= (worksheet.Dimension?.Rows ?? 0); i++)
        {
            var isEmptyRow = true;

            for (var j = 1; j <= (worksheet.Dimension?.Columns ?? 0); j++)
                if (!string.IsNullOrEmpty(worksheet.Cells[i, j].Value?.ToString()?.Trim()))
                {
                    isEmptyRow = false;
                    break;
                }

            if (!isEmptyRow) continue;
            worksheet.DeleteRow(i--);
        }

        package.Save();

        return package;
    }


    public string ConvertCsvToExcel(string fileName)
    {

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\ExcelFiles\" + fileName);

        // Load file to workbook
        Workbook workbook;
        if (Path.GetExtension(fileName) == ".csv")
        {
            var loadOption = new TxtLoadOptions(LoadFormat.Csv)
            {
                Separator = ';', // Data in CSV file is separated by semicolon
                ConvertDateTimeData = false // Do not convert date time to numeric
            };
            workbook = new Workbook(filePath, loadOption);
        }
        else
        {
            workbook = new Workbook(filePath);
        }

        filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\ExcelFiles\" +  Path.GetFileNameWithoutExtension(fileName) + ".xlsx");
        // Convert to xlsx
        using var memoryStream = new MemoryStream();
        workbook.Save(memoryStream, SaveFormat.Xlsx);

        using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        memoryStream.WriteTo(fileStream);
        
        memoryStream.Close();
        // fileStream.Seek(0, SeekOrigin.Begin);

        return filePath;
    }

    //DownloadFile from Url
    public async Task<bool> DownloadFile(string url, string savePath)
    {
        //Check if the url is in the correct format
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) && uriResult != null)
        {
            return false;
        }

        try
        {
            using (HttpResponseMessage response = await httpClient.GetAsync(uriResult, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                    stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}