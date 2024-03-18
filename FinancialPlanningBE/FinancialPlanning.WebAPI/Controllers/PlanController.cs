using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using FinancialPlanning.Service.Services;
using FinancialPlanning.WebAPI.Models.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinancialPlanning.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController(IMapper mapper, PlanService planService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly PlanService _planService = planService ?? throw new ArgumentNullException(nameof(planService));

        [HttpGet("Planlist")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _planService.GetAllPlans();
            var planListModels = plans.Select(t => _mapper.Map<PlanListModel>(t)).ToList();
            return Ok(planListModels);
        }
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetPlanById(Guid id)
        {
            var plan = await _planService.GetPlanById(id);
            return Ok(plan);
        }

        [HttpPost]
        [Authorize(Roles = "Accountant, FinancialStaff, Admin")]
        public async Task<IActionResult> CreatePlan(PlanListModel planModel)
        {
            if (ModelState.IsValid)
            {
                var plan = _mapper.Map<Plan>(planModel);
                await _planService.CreatePlan(plan);
                return Ok(new { message = "Plan created successfully!" });
            }

            return BadRequest(new { error = "Invalid model state!" });
        }







        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            await _planService.DeletePlan(id);
            return Ok(new { message = $"Plan with id {id} deleted successfully!" });
        }
    }



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