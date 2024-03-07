using System.Globalization;
using Amazon.S3;
using Amazon.S3.Model;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace FinancialPlanning.Service.Services;

public class FileService(IAmazonS3 s3Client, IConfiguration configuration)
{
    private const int MaxSize = 500 * 1024 * 1024; //500MB

    private static readonly string[] TemplatePath =
    [
        @"FinancialPlanning.Service/Template/Financial Plan_Template.xlsx",
        @"FinancialPlanning.Service/Template/Monthly Expense Report_Template.xlsx"
    ];

    private readonly string[][] _header =
    [
        [
            "DATE", "TERM", "DEPARTMENT", "EXPENSE", "COST TYPE", "UNIT PRICE", "AMOUNT", "Currency", "Exchange rate",
            "TOTAL", "", "PROJECT NAME", "SUPPLIER NAME", "PIC", "NOTE"
        ],
        [
            "DATE", "TERM", "DEPARTMENT", "EXPENSE", "COST TYPE", "UNIT PRICE", "AMOUNT", "TOTAL", "PROJECT NAME",
            "SUPPLIER NAME", "PIC", "NOTE"
        ]
    ];

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
    public async Task UploadPlanAsync(string key, FileStream fileStream)
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
        string[] validExtension = [".xls", ".xlsx", ".csv"];

        //check file is not empty, not bigger than 500MB and has valid extension
        if (fileStream.Length == 0 || fileStream.Length > MaxSize ||
            !validExtension.Contains(Path.GetExtension(fileStream.Name).ToLower()))
        {
            return false;
        }

        using var package = new ExcelPackage(fileStream);
        //check file has sheet
        if (package.Workbook.Worksheets.Count == 0) return false;

        //check file has data
        var worksheet = package.Workbook.Worksheets[0];
        var numOfRows = worksheet.Dimension.Rows;
        if (numOfRows < 3) return false;

        //check number of column is sufficient
        var numOfColumns = worksheet.Dimension.Columns;
        if (numOfColumns != _header[documentType].Length) return false;

        //check cell content is not null and has valid format
        for (var i = 1; i <= numOfColumns; i++)
        {
            if (!(worksheet.Cells[2, i].Value?.ToString() ?? "").Equals(_header[documentType][i])) return false;

            for (var j = 3; j <= numOfRows; j++)
            {
                var cellContext = worksheet.Cells[j, i].Value?.ToString()?.Trim();
                if (cellContext.IsNullOrEmpty()) return false;

                switch (i)
                {
                    case 1:
                        if (!DateTime.TryParseExact(cellContext!, "dd/MM/yyyy", null, DateTimeStyles.None, out _))
                            return false;
                        break;
                    case 6:
                    case 7:
                    case 8 when documentType != 0:
                    case 10 when documentType == 0:
                        if (!decimal.TryParse(cellContext!, NumberStyles.Number, null, out _))
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
        using var package = new ExcelPackage(fileStream);
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
                ExchangeRate = documentType == 0 ? double.Parse(worksheet.Cells[i, 9].Value.ToString()!.Trim()) : null,
                TotalAmount = decimal.Parse(worksheet.Cells[i, 10].Value.ToString()!.Trim()),
                ProjectName = worksheet.Cells[i, 12].Value.ToString()!.Trim(),
                SupplierName = worksheet.Cells[i, 13].Value.ToString()!.Trim(),
                PIC = worksheet.Cells[i, 14].Value.ToString()!.Trim(),
                Note = worksheet.Cells[i, 15].Value.ToString()?.Trim()
            });
        }

        return expenses;
    }

    public async Task<Stream> ConvertListToExcelAsync(IEnumerable<Expense> expenses, byte documentType)
    {
        //Write list of expenses to ExcelPackage
        using var package = new ExcelPackage(new FileInfo(TemplatePath[documentType]));
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

            worksheet.Cells[index + 2, 10].Value = expense.TotalAmount;
            worksheet.Cells[index + 2, 12].Value = expense.ProjectName;
            worksheet.Cells[index + 2, 13].Value = expense.SupplierName;
            worksheet.Cells[index + 2, 14].Value = expense.PIC;
            worksheet.Cells[index + 2, 15].Value = expense.Note;
        }

        //Convert ExcelPackage to Stream
        var memoryStream = new MemoryStream();
        await package.SaveAsAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}