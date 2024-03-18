using System;
using System.IO;
using System.Threading.Tasks;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
namespace FinancialPlanning.Service.Services
{
    public class PlanService
    {
        private readonly FileService _fileService;
        private readonly IPlanRepository _planRepository;

        public PlanService(IPlanRepository planRepository, FileService fileService)
        {
            _planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
        }

        public bool ValidatePlanFileAsync(FileStream fileStream)
        {
            // Validate the file using FileService
            try
            {
                // Assuming plan documents have document type 0
                return _fileService.ValidateFile(fileStream, documentType: 0);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while validating the plan file.", ex);
            }
        }




        public async Task<IEnumerable<Plan>> GetStartingPlans()
        {
            IEnumerable<Plan> plans = await _planRepository.GetAllPlans();
            List<Plan> startingPlans = [];
            foreach (var plan in plans)
            {
                {
                    startingPlans.Add(plan);
                }
            }
            return startingPlans;
        }
        

        public async Task<Plan> GetPlanById(Guid id)
        {
            return await _planRepository.GetPlanById(id);
        }


        public async Task CreatePlan(Plan plan)
        {
            await _planRepository.CreatePlan(plan);
        }

        public async Task<List<Plan>> GetFinancialPlans(string keyword = "", string department = "", string status = "")
        {
          return  await _planRepository.GetFinancialPlans(keyword, department, status);   
        }




        public async Task UpdatePlan(Plan plan)
        {
            var existingPlan = await _planRepository.GetPlanById(plan.Id) ?? throw new ArgumentException("Plan not found with the specified ID");

            var Status = existingPlan.Status;
            if (Status == 1)
            {
                existingPlan.PlanName = plan.PlanName;
                existingPlan.PlanVersions = plan.PlanVersions;
                existingPlan.Status = plan.Status;
                existingPlan.Department = plan.Department;
                existingPlan.Term = plan.Term;
                await _planRepository.UpdatePlan(plan);
            }
            else
            {
                throw new ArgumentException("Plan cannot be updated as it is not in the new status");
            }
        }

        public async Task DeletePlan(Guid id)
        {
            var planToDelete = await _planRepository.GetPlanById(id);
            if (planToDelete != null)
            {
                await _planRepository.DeletePlan(planToDelete);
            }
            else
            {
                throw new ArgumentException("Plan not found with the specified ID");
            }
        }

        public async Task<IEnumerable<Plan>> GetAllPlans()
        {
            return await _planRepository.GetAllPlans();
        }

        public async Task ClosePlans()
        {
            IEnumerable<Plan> plans = await _planRepository.GetAllPlans();
        }

        public string ConvertFile(String fileName)
        {
            // Convert the file to a list of expenses using FileService
            try
            {
                // Assuming plan documents have document type 0
                return _fileService.ConvertCsvToExcel(fileName);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while importing the plan file.", ex);
            }

        }

        public List<Expense> GetExpenses(FileStream fileStream)
        {
            try
            {
                // Convert the file to a list of expenses using FileService
                var expenses = _fileService.ConvertExcelToList(fileStream, documentType: 0);
                return expenses;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while importing the plan file.", ex);
            }
        }
    
        public async Task SavePlan(Term term, Guid creatorId)
        {
            // Save the plan using PlanRepository
            try
            {
                Plan plan = new()
                {
                    TermId = term.Id,
                    DepartmentId = Guid.Parse("9F1EB9E2-C15B-4E5B-A2D1-6CD3D783CE73"),
                    // Status = 0,
                    // PlanVersions = new List<PlanVersion>()
                };
                await _planRepository.SavePlan(plan, creatorId);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while saving the plan.", ex);
            }
        }
    }
}
