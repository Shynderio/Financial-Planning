﻿using FinancialPlanning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public interface IReportRepository
    {
        public Task<List<Report>> GetAllReports();
        public Task<List<Report>> GetReportsByDepartId(Guid departId);
        public Task DeleteReport(Report report);
        public Task DeleteReportVersions(IEnumerable<ReportVersion> reportVersions);
        public Task<Report?> GetReportById(Guid id);
        public Task<List<ReportVersion>> GetReportVersionsByReportID(Guid reportId);
        public Task<bool> IsReportExist(Guid termId, Guid departmentId, string month);
        public Task CreateReport(Report report, Guid userId);
        public Task ReupReport(Guid reportId, Guid userId);
        public Task<List<Report>> GetAllDueReports();
        public Task CloseAllDueReports(List<Report> reports);
    }
}
