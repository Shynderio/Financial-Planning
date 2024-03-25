using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;

namespace FinancialPlanning.Service.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly FileService _fileService;
        public ReportService(IReportRepository reportRepository,
         IAuthRepository authRepository, IDepartmentRepository departmentRepository,FileService fileService)
        {
            _reportRepository = reportRepository;  
            _fileService = fileService;
            _authRepository = authRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Report>> GetReportsByEmail(string email)
        {
            var role = await _authRepository.GetRoleUser(email);

            //If role is FinancialStaff 
            if (role.Equals("FinancialStaff"))
            {
                //Get departmentId
                var departIdRaw = await _departmentRepository.GetDepartmentIdByEmail(email);
                Console.WriteLine(departIdRaw);
                var src = departIdRaw;
                var departId = Guid.Parse(src);
                //Get report by DepartmentId
                var reports =  await _reportRepository.GetReportsByDepartId(departId);
                return reports;
            }
            else
            {
                //If role is accountant - getAll
                var reports = await _reportRepository.GetAllReports();
                return reports;
            }

        }

        public async Task DeleteReport(Guid id)
        {
            var reportToDelete = await _reportRepository.GetReportById(id);
            if (reportToDelete != null)
            {
                await _reportRepository.DeleteReportVersions(reportToDelete.ReportVersions!);
                await _reportRepository.DeleteReport(reportToDelete);
            }
            else
            {
                throw new ArgumentException("Report not found with the specified ID");
            }
        }
        public async Task<IEnumerable<Department>> GetAllDepartment()
        {
            return await _departmentRepository.GetAllDepartment();
        }

        public async Task<string> GetFileByName(string key)
        {
            return await _fileService.GetFileAsync(key);
        }

        public async Task<Report?> GetReportById(Guid id)
        {
            var report = await _reportRepository.GetReportById(id);
            return report;
        }

        public async Task<IEnumerable<ReportVersion>> GetReportVersionsAsync(Guid reportId)
        {
            var repostVersions = await _reportRepository.GetReportVersionsByReportID(reportId);
            return repostVersions;
        }

        public bool ValidateReportFileData(Byte[] fileByte)
        {
            // Validate the file using FileService
            throw new NotImplementedException();
            // try
            // {
            //     // Assuming plan documents have document type 0
            //     return _fileService.ValidateFile(fileByte, documentType: 1);
            // }
            // catch (Exception ex)
            // {
            //     // Log exception
            //     throw new InvalidOperationException("An error occurred while validating the plan file.", ex);
            // }
        }

        public async Task CreateReport(Report report, Guid userId)
        {
            var isReportExist = await _reportRepository.IsReportExist(report.TermId, report.DepartmentId, report.Month);
            if (isReportExist)
            {
                throw new ArgumentException("Report already exists with the specified term, department and month");
            } else {
                report.Status = (int)Common.ReportStatus.New;
                report.UpdateDate = DateTime.Now;
                await _reportRepository.CreateReport(report, userId);
            }
            // await _reportRepository.CreateReport(report);
        }

        public async Task ReupReport(Guid reportId, Guid userId)
        {
            var isReportExist = await _reportRepository.GetReportById(reportId);
            if (isReportExist == null)
            {
                throw new ArgumentException("Report not found with the specified ID");
            } else {
                await _reportRepository.ReupReport(reportId, userId);
            }
        }

        public async Task CloseDueReports()
        {
            var reports = await _reportRepository.GetAllDueReports();
            await _reportRepository.CloseAllDueReports(reports);
        }
    }
}

