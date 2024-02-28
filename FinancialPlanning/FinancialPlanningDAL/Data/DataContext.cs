using FinancialPlanningDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanningDAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        DbSet<Term> Terms { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Plan> Plans { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<PlanVersion> PlanVersions { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<ReportVersion> ReportVersions { get; set; }
        DbSet<User> Users { get; set; }

    }
}
