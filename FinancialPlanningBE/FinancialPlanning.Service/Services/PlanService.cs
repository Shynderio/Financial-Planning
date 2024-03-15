using System;
using System.IO;
using System.Threading.Tasks;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
namespace FinancialPlanning.Service.Services
{
    public class PlanService
    {
        private readonly FileService _fileService;

        public PlanService(FileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
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
    }
}
