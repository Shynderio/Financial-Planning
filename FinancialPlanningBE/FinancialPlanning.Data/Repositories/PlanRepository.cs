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
    public class PlanRepository(DataContext context) : IPlanRepository
    {
        private readonly DataContext _dbContext = context;

        public Task<bool> Approve(Guid termId, string planName, string departmentOrUid, string file)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreatePlan(Plan plan)
        {

            plan.Id = Guid.NewGuid();
            _dbContext.Plans!.Add(plan);
            await _dbContext.SaveChangesAsync();
            return plan.Id;
        }

        public async Task DeletePlan(Plan plan)
        {

            _dbContext.Plans!.Remove(plan);
            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> ExportPlan(string file)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Plan>> GetAllPlans()
        {
            return await _dbContext.Plans!.ToListAsync();
        }

        public async Task<List<Plan>> GetPlanByDepartId(Guid departmentId)
        {
            return await _dbContext.Plans!.Where(p => p.DepartmentId == departmentId).ToListAsync();

        }

        public async Task<Plan> GetPlanById(Guid id)
        {

            var plan = await _dbContext.Plans!.FindAsync(id) ?? throw new Exception("Plan not found");
            return plan;
        }

        public Task<Plan> GetPlanDetails(Guid termId, string department, int version)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Plan>> GetPlans(Guid? termId, Guid? departmentId)
        {
            // Kiểm tra xem termId hoặc departmentId có null không
            if (termId == null && departmentId == null)
            {
                // Trong trường hợp cả hai là null, không thực hiện tìm kiếm, trả về danh sách rỗng hoặc xử lý một cách phù hợp
                return await _dbContext.Plans!.ToListAsync();

            }

            // Sử dụng termId hoặc departmentId (nếu có) để truy vấn danh sách các kế hoạch từ cơ sở dữ liệu
            var query = _dbContext.Plans!.AsQueryable();

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

        public Task<bool> ImportPlan(string uid, Guid termId, string planName, string file)
        {
            throw new NotImplementedException();
        }


        public Task<bool> ReupPlan(string uid, Guid termId, string planName, string file)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubmitPlan(Guid termId, string planName, string departmentOrUid)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePlan(Plan plan)
        {
            var existingTerm = await _dbContext.Plans!.FindAsync(plan.Id) ?? throw new Exception("Plan not found");
            existingTerm.PlanName = plan.PlanName;
            existingTerm.PlanVersions = plan.PlanVersions;
            existingTerm.Status = plan.Status;
            existingTerm.Term = plan.Term;
            existingTerm.Department = plan.Department;

            await _dbContext.SaveChangesAsync();
        }

        public Task<Plan> ViewPlan(string file)
        {
            throw new NotImplementedException();
        }

    }
}
