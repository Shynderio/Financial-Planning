using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly DataContext _context;

        public PlanRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreatePlan(Plan plan)
        {
            plan.Id = Guid.NewGuid();
            _context.Plans!.Add(plan);
            await _context.SaveChangesAsync();
            return plan.Id;
        }

        public async Task DeletePlan(Plan plan)
        {
            _context.Plans!.Remove(plan);
            await _context.SaveChangesAsync();
        }

        public Task<bool> Approve(Guid termId, string planName, string departmentOrUid, string file)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Plan>> GetAllPlans()
        {
            return await _context.Plans!.ToListAsync();
        }

        public async Task<List<Plan>> GetPlanByDepartId(Guid departmentId)
        {
            return await _context.Plans!.Where(p => p.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<Plan> GetPlanById(Guid id)
        {
            return await _context.Plans!.FindAsync(id) ?? throw new Exception("Plan not found");
        }

        public async Task<List<Plan>> GetPlans(Guid? termId, Guid? departmentId)
        {
            // Kiểm tra xem termId hoặc departmentId có null không
            if (termId == null && departmentId == null)
            {
                // Trong trường hợp cả hai là null, không thực hiện tìm kiếm, trả về danh sách rỗng hoặc xử lý một cách phù hợp
                return await _context.Plans!.ToListAsync();

            }

            // Sử dụng termId hoặc departmentId (nếu có) để truy vấn danh sách các kế hoạch từ cơ sở dữ liệu
            var query = _context.Plans!.AsQueryable();

            if (termId != null)
            {
                query = query.Where(p => p.TermId == termId);
            }

            if (departmentId != null)
            {
                query = query.Where(p => p.DepartmentId == departmentId);
            }

            // Thực hiện truy vấn và trả về danh sách kế hoạch
            return await query.ToListAsync();

        }

        public async Task<Plan> SavePlan(Plan plan, Guid creatorId)
        {
            var existingPlan = await _context.Plans!
                .Include(p => p.Term)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.TermId == plan.TermId && p.DepartmentId == plan.DepartmentId);
            if (existingPlan != null)
            {
                var newVersion = _context.PlanVersions!.Count(pv => pv.PlanId == existingPlan.Id) + 1;
                var planVersion = new PlanVersion
                {
                    Id = Guid.NewGuid(),
                    PlanId = existingPlan.Id,
                    Version = newVersion,
                    CreatorId = creatorId,
                    ImportDate = DateTime.UtcNow
                };

                _context.PlanVersions!.Add(planVersion);
                _context.SaveChanges();
                
                return existingPlan;
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

                await _context.SaveChangesAsync();

                return _context.Plans!
                    .Include(p => p.Term)
                    .Include(p => p.Department).FirstOrDefault(p => p.Id == plan.Id)!;
            }
        }

        public Task<Plan> ViewPlan(string file)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImportPlan(string uid, Guid termId, string planName, string file)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReupPlan(string uid, Guid termId, string planName, string file)
        {
            throw new NotImplementedException();
        }

        public Task<Plan> GetPlanDetails(Guid termId, string department, int version)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubmitPlan(Guid termId, string planName, string departmentOrUid)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPlanRepository.Approve(Guid termId, string planName, string departmentOrUid, string file)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExportPlan(string file)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePlan(Plan plan)
        {
            var existingTerm = await _context.Plans!.FindAsync(plan.Id) ?? throw new Exception("Plan not found");
            existingTerm.PlanName = plan.PlanName;
            existingTerm.PlanVersions = plan.PlanVersions;
            existingTerm.Status = plan.Status;
            existingTerm.Term = plan.Term;
            existingTerm.Department = plan.Department;

            await _context.SaveChangesAsync();
        }

    }
}
