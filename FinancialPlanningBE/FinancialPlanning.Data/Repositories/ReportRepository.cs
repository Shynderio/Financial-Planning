using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext context;
        public ReportRepository(DataContext context)
        {
            this.context = context;
        }

        //Get all reports
        public async Task<List<Report>> GetAllReports()
        {
            var reports = new List<Report>();
            if (context.Reports!=null)
            {
             reports = await context.Reports
                    .Where(r => r.Status > 0)
                 .OrderBy(r => r.Status) // order by status
                .ThenByDescending(r => r.UpdateDate)
                .Include(t => t.ReportVersions)
                .Include(t => t.Term)
                .Include(t => t.Department).ToListAsync();          
            }

            return reports;
        }

        //get report by department ID
        public async Task<List<Report>> GetReportsByDepartId(Guid departId)
        {
            var reports = await context.Reports!
                .Where(r => r.DepartmentId == departId && r.Status>0)
                .OrderBy(r => r.Status) // order by status
                .ThenByDescending(r => r.UpdateDate)
                .Include(t => t.ReportVersions)
                .Include(t => t.Term)
                .Include(t => t.Department)
                .ToListAsync();
    
            return reports;
        }
        //Delete report
        public async Task DeleteReport(Report report)
        {
             context.Reports!.Remove(report);
            await context.SaveChangesAsync();
        }

        //Delete report Version
        public async Task DeleteReportVersions(IEnumerable<ReportVersion> reportVersions)
        {
            foreach (var reportVersion in reportVersions)
            {
                context.ReportVersions!.Remove(reportVersion);
            }
            await context.SaveChangesAsync();
        }

        public async Task<Report> GetReportById(Guid id)
        {
            var report = await context.Reports!
         .Include(t => t.ReportVersions)
         .Include(t => t.Term)
         .Include(t => t.Department)
         .FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception("Report not found");
            return report;
        }

        //Get list reportVersions 
        public async Task<List<ReportVersion>> GetReportVersionsByReportID(Guid reportId)
        {
           var reportVersions = await context.ReportVersions!
                .Where(r => r.ReportId == reportId)
                .OrderByDescending(r => r.Version)
                .Include(r => r.User).ToListAsync();
            return reportVersions;
        }
    }
}
