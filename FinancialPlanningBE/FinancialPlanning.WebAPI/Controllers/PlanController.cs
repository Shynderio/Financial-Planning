using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinancialPlanning.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController(PlanService planService) : ControllerBase
    {
        private readonly PlanService _planService = planService;

        // POST: api/plan
        [Authorize(Roles = "FinancialStaff")]
        [HttpPost("import")]
        public async Task<ActionResult<List<Expense>>> Import(IFormFile file, string user)
        {
            try
            {
                // Check if a file is uploaded
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                // Generate a unique filename using GUID and original file extension
                var tempFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                // Save the uploaded file to the temporary directory
                var tempFilePath = Path.Combine("Resources", "ExcelFiles", tempFileName);
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Convert the file
                string convertedFilePath;
                try
                {
                    convertedFilePath = _planService.ConvertFile(tempFileName);
                }
                catch
                {
                    // Delete the temporary file if conversion fails
                    System.IO.File.Delete(tempFilePath);
                    return BadRequest("Failed to convert");
                }

                // Validate the file
                bool isValid;
                using (var openfileStream = new FileStream(convertedFilePath, FileMode.Open))
                {
                    isValid = _planService.ValidatePlanFileAsync(openfileStream);
                }

                if (!isValid)
                {
                    // Delete the temporary file if validation fails
                    System.IO.File.Delete(convertedFilePath);
                    System.IO.File.Delete(tempFilePath);
                    return BadRequest("Invalid file format");
                }

                // Get expenses
                List<Expense> expenses;
                using (var openfileStream = new FileStream(convertedFilePath, FileMode.Open))
                {
                    expenses = _planService.GetExpenses(openfileStream);
                }

                // Delete temporary files
                System.IO.File.Delete(convertedFilePath);
                System.IO.File.Delete(tempFilePath);

                return Ok(expenses);
            }
            catch (Exception)
            {
                // Log the exception
                // It's generally not a good practice to return detailed exception messages to clients
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while importing the plan file.");
            }
        }
    }
}