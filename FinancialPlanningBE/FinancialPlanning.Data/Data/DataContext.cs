using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPlanning.Data.Entities;

namespace FinancialPlanning.Data.Data
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
        public DbSet<Term>? Terms { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<Plan>? Plans { get; set; }
        public DbSet<Position>? Positions { get; set; }
        public DbSet<PlanVersion>? PlanVersions { get; set; }
        public DbSet<Report>? Reports { get; set; }
        public DbSet<ReportVersion>? ReportVersions { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Role> Roles { get; set; }   
    }
}
