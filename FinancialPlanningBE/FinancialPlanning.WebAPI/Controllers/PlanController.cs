using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using FinancialPlanning.WebAPI.Models.Plan;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;
using PlanStatus = FinancialPlanning.Common.PlanStatus;
using FinancialPlanning.WebAPI.Models.Expense;

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
        [HttpPost("import")]
        [Authorize(Roles = "FinancialStaff")]
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
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                if (Path.GetExtension(file.FileName).Equals(".csv"))
                    memoryStream = fileService.ConvertCsvToExcel(memoryStream);
                var isValid = _planService.ValidatePlanFile(memoryStream.ToArray());

                if (!isValid)
                {
                    memoryStream.Close();
                    return BadRequest(new { message = "Invalid file format!" });
                }

                // Get expenses
                var expenses = _planService.GetExpenses(memoryStream.ToArray());
                memoryStream.Close();
                
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

        // POST: api/plan/
        [HttpPost("reup")]
        [Authorize(Roles = "FinancialStaff")]
        public async Task<ActionResult<List<ExpenseStatusModel>>> ReuploadPlan(IFormFile file, Guid planId)
        {
            try
            {
                // Check if a file is uploaded
                if (file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                // Validate the file
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                if (Path.GetExtension(file.FileName).Equals(".csv"))
                    memoryStream = fileService.ConvertCsvToExcel(memoryStream);
                var isValid = _planService.ValidatePlanFile(memoryStream.ToArray());

                if (!isValid)
                {
                    memoryStream.Close();
                    return BadRequest(new { message = "Invalid file format!" });
                }

                // Get expenses
                var expenses = _planService.GetExpenses(memoryStream.ToArray());
                memoryStream.Close();
                
                // Check expenses
                var approvedExpenses = _planService.CheckExpenses(expenses, planId);


                var expenseModels = new List<ExpenseStatusModel>();

                var planStatus = _planService.GetPlanById(planId).Result!.Status;
                foreach (var expense in expenses)
                {
                    var expenseModel = _mapper.Map<ExpenseStatusModel>(expense);
                    if (planStatus != PlanStatus.New)
                    {
                        expenseModel.Status = approvedExpenses.Contains(expense.No)
                            ? PlanStatus.Approved
                            : PlanStatus.WaitingForApproval;
                    }
                    else
                    {
                        expenseModel.Status = PlanStatus.New;
                    }

                    expenseModels.Add(expenseModel);
                }

                return Ok(expenseModels);
            }
            catch (Exception ex)
            {
                // Log the exception
                // It's generally not a good practice to return detailed exception messages to clients
                // return StatusCode(StatusCodes.Status500InternalServerError,
                //     "An error occurred while importing the plan file.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("details/{id:guid}")]
       // [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetPlanDetails(Guid id)
        {
            try
            {
                //Get plan
                var plan = await _planService.GetPlanById(id);
                string filename = plan.Department.DepartmentName + "/"
                      + plan.Term.TermName + "/Plan/version_" + plan.GetMaxVersion() +".xlsx";
                //Get planVersions
                var planVersions = await _planService.GetPlanVersionsAsync(id);
                var expenses = _fileService.ConvertExcelToList(await _fileService.GetFileAsync(filename), 0);
                //mapper
                var planViewModel = _mapper.Map<PlanViewModel>(plan);
                var planVersionModel = _mapper.Map<IEnumerable<PlanVersionModel>>(planVersions).ToList();
                // Get the name of the user who uploaded the file
                var firstPlanVersion = planVersionModel.FirstOrDefault();
                var uploadedBy = firstPlanVersion?.UploadedBy;
                var dueDate = plan.Term.PlanDueDate;
                var date = firstPlanVersion?.ImportDate;

                var result = new
                {
                    Plan = planViewModel,
                    planDueDate = dueDate,
                    date = date,
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

        [HttpPost("edit")]
        [Authorize(Roles = "FinancialStaff")]
        public async Task<IActionResult> EditPlan(List<ExpenseStatusModel> expenseModels, Guid planId, Guid userId)
        {
            try
            {
                var expenses = new List<Expense>();
                var approvedNos = new List<int>();
                foreach (var expenseModel in expenseModels)
                {
                    var expense = _mapper.Map<Expense>(expenseModel);
                    expenses.Add(expense);
                    if (expenseModel.Status == PlanStatus.Approved)
                    {
                        approvedNos.Add(expense.No);
                    }
                }

                await _planService.ReupPlan(expenses, approvedNos, planId, userId);
                return Ok(new { message = "Plan updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.Message}");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "FinancialStaff")]
        public async Task<IActionResult> CreatePlan(List<Expense> expenses, Guid termId, Guid uid)
        {
            try
            {
                var plan = new Plan
                {
                    TermId = termId,
                    Status = PlanStatus.New,
                };
                await _planService.CreatePlan(expenses, plan, uid);
                return Ok(new { message = "Plan updated successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.Message}");
            }
        }
        [HttpGet("export/{id:guid}/{version:int}")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> ExportSinglePlan(Guid id, int version)
        {
            try
            {
                //from planVersion Id -> get name plan + version
                var plan = await _planService.GetPlanById(id);
                string filename = plan.Department.DepartmentName + "/"
                      + plan.Term.TermName + "/Plan/version_"
                      + version;


                //get url from name file
                var url = await _planService.GetFileByName(filename + ".xlsx");

                // return URL
                return Ok(new { downloadUrl = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex });
            }
        }
    }
}