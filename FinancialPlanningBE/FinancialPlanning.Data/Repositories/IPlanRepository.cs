using FinancialPlanning.Data.Entities;

namespace FinancialPlanning.Data.Repositories
{

    // Interface for managing financial plans.
    public interface IPlanRepository
    {
        
        public Task<List<Plan>> GetFinancialPlans(string keyword = "", string department = "", string status = "");
        public Task<List<Plan>> GetAllPlans();
        public Task<List<Plan>> GetPlanByDepartId(Guid departmentID);
        // public Task<Plan> ViewPlan(string file);
        public Task<Plan> ImportPlan(Plan plan, Guid userId);
        public Task<Plan> ReupPlan(Plan plan, Guid userId);
        // public Task<Plan> GetPlanDetails(Guid termId, string department, int version);
        public Task<List<Plan>> GetPlans(Guid? termId, Guid? departmentId);
        public Task<bool> SubmitPlan(Guid termId, string planName, string departmentOrUid);
        // public Task<bool> Approve(Guid termId, string planName, string departmentOrUid, string file);
        // public Task<bool> ExportPlan(string file);
        public Task<Plan?> GetPlanById(Guid id);
        public Task<Guid> CreatePlan(Plan plan);
        public Task UpdatePlan(Plan plan);
        public Task DeletePlan(Plan plan);
        public Task<Plan> SavePlan(Plan plan, Guid creatorId);
        public Task<List<Plan>> GetAllDuePlans();
        public Task CloseAllDuePlans(List<Plan> plans);
        public Task<List<PlanVersion>> GetPlanVersionsByPlanID(Guid planId);
        public Task<bool> IsPlanExist(Guid termId, Guid departmentId);
    }

}
