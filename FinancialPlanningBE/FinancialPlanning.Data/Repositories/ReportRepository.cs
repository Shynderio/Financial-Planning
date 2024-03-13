using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<Report>> GetAllReports()
        {
            var reports = new List<Report>();
            if (context.Reports!=null)
            {
             reports = await context.Reports.ToListAsync();
          
            }

            return reports;
        }
        public async Task<List<Report>> GetReportsByDepartId()
        {
           var reports = await context.Reports.ToListAsync();
            return reports;
        }

    }
}
