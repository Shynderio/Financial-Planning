using FinancialPlanningDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): 
            base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException("modelBuilder");

            // for the other conventions, we do a metadata model loop
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.SetTableName(entityType.DisplayName());

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

            base.OnModelCreating(modelBuilder);
        }
        internal DbSet<Term>? Terms { get; set; }
        internal DbSet<Department>? Departments { get; set; }
        internal DbSet<Plan>? Plans { get; set; }
        internal DbSet<Position>? Positions { get; set; }
        internal DbSet<PlanVersion>? PlanVersions { get; set; }
        internal DbSet<Report>? Reports { get; set; }
        internal DbSet<ReportVersion>? ReportVersions { get; set; }
        internal DbSet<User>? Users { get; set; }
        internal DbSet<Role>? Roles { get; set; }   
    }
}
