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
        public AnnualReportController(FileService fileService)
        {
            _fileService = fileService;
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
                            var (expense, reports) = _fileService.ConvertExelAnnualReport(package);
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

       
    }




}
