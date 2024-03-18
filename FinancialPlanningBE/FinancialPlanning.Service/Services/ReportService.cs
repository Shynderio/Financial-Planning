using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IConfiguration configuration;
        private readonly IAuthRepository _authRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public ReportService(IReportRepository reportRepository, IConfiguration configuration, IAuthRepository authRepository, IDepartmentRepository departmentRepository)
        {
            _reportRepository = reportRepository;
            this.configuration = configuration;
            _authRepository = authRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Report>> GetReportsByEmail(string email)
        {
            var role = await _authRepository.GetRoleUser(email);

            //If role is FinancialStaff 
            if (role.ToString().Equals("FinancialStaff"))
            {
                //Get departmentId
                var departID_raw = await _departmentRepository.GetDepartmentIdByEmail(email);
                Console.WriteLine(departID_raw.ToString());
                string src = departID_raw.ToString();
                Guid departId = Guid.Parse(src);
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

        public async Task<List<Department>> GetAllDepartment()
        {
            return await _departmentRepository.GetAllDepartment();
        }

    }
}

