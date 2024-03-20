using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;

namespace FinancialPlanning.Service.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ReportService(IReportRepository reportRepository, IAuthRepository authRepository, IDepartmentRepository departmentRepository)
        {
            _reportRepository = reportRepository;  
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



    }
}

