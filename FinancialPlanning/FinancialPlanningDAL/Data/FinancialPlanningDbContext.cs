using FinancialPlanningDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Data
{
    public class FinancialPlanningDbContext : DbContext
    {
        public FinancialPlanningDbContext(DbContextOptions<FinancialPlanningDbContext> options): 
            base(options) { }

        DbSet<Term>? Terms { get; set; }
        DbSet<Department>? Departments { get; set; }
        DbSet<Plan>? Plans { get; set; }
        DbSet<Position>? Positions { get; set; }
        DbSet<PlanVersion>? PlanVersions { get; set; }
        DbSet<Report>? Reports { get; set; }
        DbSet<ReportVersion>? ReportVersions { get; set; }
        DbSet<User>? Users { get; set; }

    }
}
