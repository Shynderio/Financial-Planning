using System.Globalization;
using Amazon.S3;
using Amazon.S3.Model;
using ExcelDataReader;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinancialPlanning.Service.Services;

public class FileService(IAmazonS3 s3Client, IConfiguration configuration)
{
    private const int MaxSize = 500 * 1024 * 1024; //500MB

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

    public async Task<string> GetFile(string key)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = configuration["AWS:BucketName"],
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            return await s3Client.GetPreSignedURLAsync(request);
        }
        catch (AmazonS3Exception)
        {
            throw new ArgumentException("File does not exist!");
        }
    }

    /*
     * documentType:
     * {
     *  plan:   0
     *  report: 1
     * }
     */
    public async Task UploadPlan(string key, FileStream fileStream, byte documentType)
    {
        if (!CheckFileValidation(fileStream, documentType))
        {
            throw new ArgumentException("File is invalid!");
        }

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
    private bool CheckFileValidation(FileStream fileStream, byte documentType)
    {
        string[] validExtension = [".xls", ".xlsx", ".csv"];

        //check file is not empty, not bigger than 500MB and has valid extension
        if (fileStream.Length == 0 || fileStream.Length > MaxSize ||
            !validExtension.Contains(Path.GetExtension(fileStream.Name).ToLower()))
        {
            return false;
        }

        using var excelDataReader = ExcelReaderFactory.CreateReader(fileStream);
        using var dataSet = excelDataReader.AsDataSet();

        //check file has sheet
        if (dataSet.Tables.Count == 0) return false;

        //check file has data
        var table = dataSet.Tables[0];
        if (table.Rows.Count < 3) return false;

        //check number of column is sufficient
        var headerRow = table.Rows[1];
        var numOfColumns = _header[documentType].Length;
        if (headerRow.ItemArray.Length != numOfColumns) return false;

        //check cell content is not null and has valid format
        var numOfRows = table.Rows.Count;
        for (var i = 0; i < numOfColumns; i++)
        {
            if (!table.Rows[1].ItemArray[i]!.ToString()!.Trim().Equals(_header[documentType][i])) return false;

            for (var j = 2; j < numOfRows; j++)
            {
                var cellContext = table.Rows[j].ItemArray[i]?.ToString()?.Trim();
                if (cellContext.IsNullOrEmpty()) return false;

                switch (i)
                {
                    case 0:
                        if (!DateTime.TryParseExact(cellContext!, "dd/MM/yyyy", null, DateTimeStyles.None, out _))
                            return false;
                        break;
                    case 5:
                    case 6:
                    case 9:
                        if (!decimal.TryParse(cellContext!, NumberStyles.Number, null, out _))
                            return false;
                        break;
                }
            }
        }

        //check TotalAmount = UnitPrice * Amount
        for (var i = 2; i < numOfRows; i++)
        {
            var unitPrice = decimal.Parse(table.Rows[i].ItemArray[5]!.ToString()!);
            var amount = decimal.Parse(table.Rows[i].ItemArray[6]!.ToString()!);
            var totalAmount = decimal.Parse(table.Rows[i].ItemArray[9]!.ToString()!);
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
        //check file is valid
        if (!CheckFileValidation(fileStream, documentType))
        {
            throw new ArgumentException("File is invalid!");
        }

        using var excelDataReader = ExcelReaderFactory.CreateReader(fileStream);
        using var dataSet = excelDataReader.AsDataSet();

        var expenses = new List<Expense>();
        var table = dataSet.Tables[0];
        for (var i = 2; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            expenses.Add(new Expense
            {
                No = i - 1,
                Date = DateTime.Parse(row.ItemArray[0]!.ToString()!.Trim()),
                Term = row.ItemArray[1]!.ToString()!.Trim(),
                Department = row.ItemArray[2]!.ToString()!.Trim(),
                ExpenseName = row.ItemArray[3]!.ToString()!.Trim(),
                CostType = row.ItemArray[4]!.ToString()!.Trim(),
                UnitPrice = decimal.Parse(row.ItemArray[5]!.ToString()!.Trim()),
                Amount = decimal.Parse(row.ItemArray[6]!.ToString()!.Trim()),
                Currency = row.ItemArray[7]!.ToString()!.Trim(),
                ExchangeRate = double.Parse(row.ItemArray[8]!.ToString()!.Trim()),
                TotalAmount = decimal.Parse(row.ItemArray[9]!.ToString()!.Trim()),
                ProjectName = row.ItemArray[10]!.ToString()!.Trim(),
                SupplierName = row.ItemArray[11]!.ToString()!.Trim(),
                PIC = row.ItemArray[12]!.ToString()!.Trim(),
                Note = row.ItemArray[13]?.ToString()?.Trim(),
                Status = Enum.Parse<Status>(row.ItemArray[14]!.ToString()!.Trim())
            });
        }

        return expenses;
    }
}