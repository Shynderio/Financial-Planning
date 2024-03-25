using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Globalization;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualReportController : ControllerBase
    {

        private readonly FileService _fileService;
        private readonly AnnualReportService _annualReportService;
        public AnnualReportController(FileService fileService, AnnualReportService annualReportService)
        {
            _fileService = fileService;
            _annualReportService = annualReportService;
        }

        [HttpGet("annualreports")]
        public async Task<IActionResult> GetAll()
        {
            var annualReport = await _annualReportService.GetAllAnnualReportsAsync();
            return Ok(annualReport);

        }

        [HttpGet("details/{year:int}")]
        public async Task<IActionResult> GetAnnualReportDetails(int year)
        {
            try
            {
                var (expense, reports) = await _annualReportService.GetAnnualReportDetails(year);
                return Ok(new { Expense = expense, Reports = reports });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        




            [HttpGet("0")]
        public async Task<IActionResult> GetAllFile(string key)
        {
            var url = await _fileService.GetFileUrlAsync(key);
            return Ok(url);

        }

        [HttpPost("ConvertAnnualReport")]
        public IActionResult ConvertAnnualReport(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var (expense, reports) = _fileService.ConvertExelAnnualReportToList(package);
                            return Ok(new { Expense = expense, Reports = reports });
                        }
                    }
                }
                else
                {
                    return BadRequest("No file uploaded.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadAnnualReport()
        {
            List<ExpenseAnnualReport> expenses = new List<ExpenseAnnualReport>();

            ExpenseAnnualReport expenseAnnualReport = new ExpenseAnnualReport
            {
                Department = "HR",
                TotalExpense = 100000000,
                BiggestExpenditure = 120000,
                CostType = "MK",
            };
            expenses.Add(expenseAnnualReport);
            var annualreport = new AnnualReport
            {
                Year = 2023,
                CreateDate = DateTime.Now,
                TotalTerm = 31,
                TotalDepartment = 12,
                TotalExpense = "1210000"
            
            };
            string filePath = Path.Combine("AnnualExpenseReport", "AnnualReport_2023.xlsx");

            //Import file to cloud
            var filae = await _fileService.ConvertAnnualReportToExcel(expenses, annualreport);


            await _fileService.UploadFileAsync(filePath.Replace('\\', '/'), new MemoryStream(filae));


            return Ok(filePath);
        }



    }




}
