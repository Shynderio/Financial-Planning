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
                .Where(r => r.DepartmentId == departId)
                 .OrderBy(r => r.Status) // order by status
                .ThenByDescending(r => r.UpdateDate)
                .Include(t => t.ReportVersions)
                .Include(t => t.Term)
                .Include(t => t.Department)
                .ToListAsync();
    
            return reports;
        }

    }
}
