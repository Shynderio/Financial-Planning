using FinancialPlanning.Common;
using Microsoft.EntityFrameworkCore;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialPlanning.Data.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<Department>().HasData(
                new Department() { Id = new Guid("0fc3fcd5-fa5a-476c-bae2-fe8af96ab59b"), DepartmentName = "IT" },
                new Department() { Id = new Guid("319bd2f6-92df-4ed2-b9d8-0ba5d8631b64"), DepartmentName = "HR" },
                new Department() { Id = new Guid("a1025f0c-092c-42a5-8770-9fb7330ec94c"), DepartmentName = "Finance" },
                new Department()
                    { Id = new Guid("2c45a186-10c0-4a45-9734-1a41afe96da8"), DepartmentName = "Communication" },
                new Department()
                    { Id = new Guid("ebaa9c87-343b-4cbc-b7b0-0aebd1cf9288"), DepartmentName = "Marketing" },
                new Department()
                    { Id = new Guid("7118524c-6e2e-48f6-ba39-b7a23dc218e6"), DepartmentName = "Accounting" });

            modelBuilder.Entity<Position>().HasData(
                new Position()
                    { Id = new Guid("487bbd92-364e-4095-b81f-7aac6841c333"), PositionName = "Financial Executive" },
                new Position()
                    { Id = new Guid("a760abe5-9533-4b2b-8884-03fca61cdcc2"), PositionName = "Project manager" },
                new Position()
                    { Id = new Guid("0ff8c462-355d-481b-a7b0-9296e956d903"), PositionName = "Office Assistant" },
                new Position()
                    { Id = new Guid("3b277498-07fb-44c5-8664-1fb7c8919fc7"), PositionName = "Senior Executive" },
                new Position()
                    { Id = new Guid("d1a75d8c-cc9c-4bb3-9cae-4ddb14dd807d"), PositionName = "Accounting Executive" },
                new Position()
                    { Id = new Guid("859ee2ca-d71a-4349-9a9f-98e4d7462834"), PositionName = "Junior Executive" },
                new Position() { Id = new Guid("1f8822ac-5f29-4063-b505-86ac60b0bd67"), PositionName = "Intern" });

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = new Guid("0816a20b-3fec-47ce-bd8d-c2dacfc608fe"), RoleName = "Admin" },
                new Role() { Id = new Guid("06468f92-1783-4fe3-a663-7c5edcf8ea98"), RoleName = "Accountant" },
                new Role() { Id = new Guid("53d8d638-6dbe-4c1f-bb00-19ee50b96b46"), RoleName = "Financial Staff" });

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.NewGuid(), Username = "SysAdmin", FullName = "SysAdmin",
                    Email = Environment.GetEnvironmentVariable("SYSADMIN_EMAIL") ?? "financialplanapp@gmail.com",
                    Password =
                        BCrypt.Net.BCrypt.HashPassword(Environment.GetEnvironmentVariable("SYSADMIN_PASSWORD") ??
                                                       "123456A@"),
                    PhoneNumber = "0123456789", Address = "Hanoi", DOB = "01/01/1990",
                    DepartmentId = new Guid("0fc3fcd5-fa5a-476c-bae2-fe8af96ab59b"),
                    PositionId = new Guid("0ff8c462-355d-481b-a7b0-9296e956d903"),
                    RoleId = new Guid("0816a20b-3fec-47ce-bd8d-c2dacfc608fe"),
                    Status = UserStatus.Active
                });

            // for the other conventions, we do a metadata model loop
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.SetTableName(entityType.DisplayName());

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => fk is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade })
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
        public DbSet<Role>? Roles { get; set; }
    }
}