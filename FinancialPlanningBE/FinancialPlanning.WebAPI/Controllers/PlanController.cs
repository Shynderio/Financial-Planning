using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinancialPlanning.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly PlanService _planService;

        public PlanController(PlanService planService)
        {
            _planService = planService ?? throw new ArgumentNullException(nameof(planService));
        }
        // POST: api/plan
        [HttpPost("import")]
        public async Task<ActionResult<List<Expense>>> Import(IFormFile file, String user)
        {
            if (file == null)
            {
                return BadRequest("No file uploaded");
            }
            try
            {
                // Save the uploaded file to a temporary location
                var filePath = Path.GetTempFileName();
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Validate the file using PlanService
                if (!_planService.ValidatePlanFileAsync(stream))
                {
                    return BadRequest("Invalid file");
                }

                // Convert the file to a list of expenses using PlanService
                var expenses = _planService.GetExpenses(stream);

                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}