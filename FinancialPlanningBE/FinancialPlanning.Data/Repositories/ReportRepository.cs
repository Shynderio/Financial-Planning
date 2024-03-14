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

        public async Task<List<Report>> GetAllReports()
        {
            var reports = new List<Report>();
            if (context.Reports!=null)
            {
             reports = await context.Reports.Include(r => r.ReportVersions).ToListAsync();
          
            }

            return reports;
        }
        public async Task<List<Report>> GetReportsByDepartId()
        {
            var reports = await context.Reports
                .Include(t => t.ReportVersions)
                .Include(t => t.Term)
                .Include(t => t.Department)
                .ToListAsync();
            //foreach (var rep in reports)
            //{
            //    var term = await context.Terms.FirstOrDefaultAsync(t => t.Id == rep.TermId);
            //    rep.Term = term;
            //}

            // Sử dụng tùy chọn ReferenceHandler.Preserve để tránh lỗi vòng lặp tham chiếu
            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    MaxDepth = 64 // Increase max depth if needed
            //};

            //// Chuyển đổi danh sách báo cáo thành chuỗi JSON với tùy chọn đã cài đặt
            //var jsonString = JsonSerializer.Serialize(reports, options);

            return reports;
        }

    }
}
