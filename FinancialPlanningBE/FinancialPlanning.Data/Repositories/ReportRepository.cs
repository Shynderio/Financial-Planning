using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanning.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _context;

        public ReportRepository(DataContext context)
        {
            this._context = context;
        }

        //Get all reports
        public async Task<List<Report>> GetAllReports()
        {
            var reports = new List<Report>();
            if (_context.Reports != null)
            {
                reports = await _context.Reports
                    .OrderBy(r => r.Status)
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
            var reports = await _context.Reports!
                .Where(r => r.DepartmentId == departId)
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
            _context.Reports!.Remove(report);
            await _context.SaveChangesAsync();
        }

        //Delete report Version
        public async Task DeleteReportVersions(IEnumerable<ReportVersion> reportVersions)
        {
            foreach (var reportVersion in reportVersions)
            {
                _context.ReportVersions!.Remove(reportVersion);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Report?> GetReportById(Guid id)
        {
            var report = await _context.Reports!
         .Include(t => t.ReportVersions)
         .Include(t => t.Term)
         .Include(t => t.Department)
         .FirstOrDefaultAsync(t => t.Id == id) ;
            return report;
        }

        //Get list reportVersions 
        public async Task<List<ReportVersion>> GetReportVersionsByReportID(Guid reportId)
        {
           var reportVersions = await _context.ReportVersions!
                .Where(r => r.ReportId == reportId)
                .OrderByDescending(r => r.Version)
                .Include(r => r.User).ToListAsync();
            return reportVersions;
        }
    }
}