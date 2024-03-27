using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using FinancialPlanning.WebAPI.Models.Plan;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;
using PlanStatus = FinancialPlanning.Common.PlanStatus;

namespace FinancialPlanning.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController(IMapper mapper, PlanService planService, FileService fileService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly PlanService _planService = planService ?? throw new ArgumentNullException(nameof(planService));
        private readonly FileService _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));


        [HttpGet]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<ActionResult<IEnumerable<PlanViewModel>>> GetFinancialPlans(string keyword = "",
            string department = "", string status = "")
        {
            var plans = await _planService.GetFinancialPlans(keyword, department, status);

            // Project the results into FinancialPlanDto
            var result = plans.Select((p, _) => new PlanViewModel
            {
                Id = p.Id,
                Plan = p.PlanName,
                Term = p.Term.TermName,
                Department = p.Department.DepartmentName,
                Status = GetPlanStatusString((PlanStatus)p.Status),
                Version = p.PlanVersions.Any()
                    ? p.PlanVersions.Max(v => v.Version)
                    : 0
            }).ToList();

            return result;
        }

        private string GetPlanStatusString(PlanStatus status)
        {
            var attribute =
                status.GetType().GetTypeInfo().GetMember(status.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)?
                    .GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? status.ToString();
        }


        [HttpGet("Planlist")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _planService.GetFinancialPlans();
            var result = plans.Select((p, _) => new PlanViewModel
            {
                Id = p.Id,
                Plan = p.PlanName,
                Term = p.Term.TermName,
                Department = p.Department.DepartmentName,
                Status = GetPlanStatusString((PlanStatus)p.Status),
                Version = p.PlanVersions.Any()
                    ? p.PlanVersions.Max(v => v.Version)
                    : 0 // Check if PlanVersions has any elements before calling Max()
            }).ToList();

            return Ok(result);
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
            if (!ModelState.IsValid) return BadRequest(new { error = "Invalid model state!" });
            var plan = _mapper.Map<Plan>(planModel);
            await _planService.CreatePlan(plan);
            return Ok(new { message = "Plan created successfully!" });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            await _planService.DeletePlan(id);
            return Ok(new { message = $"Plan with id {id} deleted successfully!" });
        }

        // POST: api/plan
        [Authorize(Roles = "FinancialStaff")]
        [HttpPost("import")]
        public async Task<ActionResult<List<Expense>>> Import(IFormFile file)
        {
            try
            {
                // Check if a file is uploaded
                if (file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                // Validate the file
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var isValid = _planService.ValidatePlanFile(memoryStream.ToArray());

                if (!isValid)
                {
                    return BadRequest(new { message = "Invalid file format!" });
                }

                // Get expenses
                var expenses = _planService.GetExpenses(memoryStream.ToArray());

                return Ok(expenses);
            }
            catch (Exception)
            {
                // Log the exception
                // It's generally not a good practice to return detailed exception messages to clients
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while importing the plan file.");
            }
        }

        [HttpPost("Upload")]
        [Authorize(Roles = "FinancialStaff")]
        public async Task<IActionResult> UploadPlan(List<Expense> expenses, Guid termId, Guid uid)
        {
            try
            {
                await _planService.SavePlan(expenses, termId, uid);
                return Ok(new { message = "Plan uploaded successfully!" });
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while uploading the plan file.");
            }
        }

        [HttpGet("details/{id:guid}")]
   //     [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetPlanDetails(Guid id)
        {
            try
            {
                //Get plan
                var plan = await _planService.GetPlanById(id);
                //Get planVersions
                var planVersions = await _planService.GetPlanVersionsAsync(id);
                var expenses = _fileService.ConvertExcelToList(await _fileService.GetFileAsync("CorrectPlan.xlsx"), 0);

                //mapper
                var planViewModel = _mapper.Map<PlanViewModel>(plan);
                var planVersionModel = _mapper.Map<IEnumerable<PlanVersionModel>>(planVersions).ToList();
                // Get the name of the user who uploaded the file
                var firstPlanVersion = planVersionModel.FirstOrDefault();
                var uploadedBy = firstPlanVersion?.UploadedBy;
                var dueDate = plan.Term.PlanDueDate;

                var result = new
                {
                    Plan = planViewModel,
                    planDueDate = dueDate,
                    Expenses = expenses,
                    PlanVersions = planVersionModel,
                    UploadedBy = uploadedBy
                };

                return Ok(result);
            }
            //error when download
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.Message}");
            }
        }
    }
}