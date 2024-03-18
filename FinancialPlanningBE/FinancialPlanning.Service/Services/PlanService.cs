using System;
using System.IO;
using System.Threading.Tasks;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Service.Services;
namespace FinancialPlanning.Service.Services
{
    public class PlanService
    {
        private readonly FileService _fileService;
        private readonly IPlanRepository _planRepository;

        public PlanService(FileService fileService, IPlanRepository planRepository)
        {
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
