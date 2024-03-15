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

        public Task<Plan> ViewPlan(string file)
        {
            throw new NotImplementedException();
        }
    }
}
