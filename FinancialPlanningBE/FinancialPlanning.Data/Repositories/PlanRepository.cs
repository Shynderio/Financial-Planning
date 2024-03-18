using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanning.Data.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly DataContext _context;

        public PlanRepository(DataContext context)
        {
            _context = context;
        }

        public Task<List<Plan>> GetAllPlans()
        {
            throw new NotImplementedException();
        }

        public Task<List<Plan>> GetPlanByDepartId()
        {
            throw new NotImplementedException();
        }

        public async Task SavePlan(Plan plan, Guid creatorId)
        {
            var existingPlan = await _context.Plans!.FirstOrDefaultAsync(p => p.TermId == plan.TermId && p.DepartmentId == plan.DepartmentId);

            if (existingPlan != null)
            {
                var newVersion = existingPlan.PlanVersions.Max(pv => pv.Version) + 1;
                var planVersion = new PlanVersion
                {
                    Id = Guid.NewGuid(),
                    PlanId = existingPlan.Id,
                    Version = newVersion,
                    CreatorId = creatorId,
                    ImportDate = DateTime.UtcNow
                };

                _context.PlanVersions!.Add(planVersion);
            }
            else
            {
                plan.Status = 0;
                plan.Id = Guid.NewGuid();

                var planVersion = new PlanVersion
                {
                    Id = Guid.NewGuid(),
                    PlanId = plan.Id,
                    Version = 1,
                    CreatorId = creatorId,
                    ImportDate = DateTime.UtcNow
                };

                plan.PlanVersions = new List<PlanVersion> { planVersion };

                _context.Plans!.Add(plan);
                _context.PlanVersions!.Add(planVersion);
            }
            
            await _context.SaveChangesAsync();
        }

    }
}