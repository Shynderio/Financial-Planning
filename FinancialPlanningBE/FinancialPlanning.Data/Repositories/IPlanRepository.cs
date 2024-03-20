using FinancialPlanning.Data.Entities;

namespace FinancialPlanning.Data.Repositories
{

    // Interface for managing financial plans.
    public interface IPlanRepository
    {
        
        public Task<List<Plan>> GetFinancialPlans(string keyword = "", string department = "", string status = "");
        public Task<List<Plan>> GetAllPlans();
        public Task<List<Plan>> GetPlanByDepartId(Guid departmentID);
        //------------


        // View details of a plan based on the file.

        public Task<Plan> ViewPlan(string file);


        // Import a new financial plan.

        public Task<bool> ImportPlan(string uid, Guid termId, string planName, string file);


        // Re-upload an existing financial plan.

        public Task<bool> ReupPlan(string uid, Guid termId, string planName, string file);


        // Retrieve details of a financial plan based on term and department.

        public Task<Plan> GetPlanDetails(Guid termId, string department, int version);


        // Retrieve a list of financial plans based on term and department for Function Search

        public Task<List<Plan>> GetPlans(Guid? termId, Guid? departmentId);


        // Submit a financial plan for approval.

        public Task<bool> SubmitPlan(Guid termId, string planName, string departmentOrUid);


        // Approve a financial plan.

        public Task<bool> Approve(Guid termId, string planName, string departmentOrUid, string file);


        // Export a financial plan to a file.

        public Task<bool> ExportPlan(string file);
        public Task<Plan?> GetPlanById(Guid id);
        public Task<Guid> CreatePlan(Plan plan);
        public Task UpdatePlan(Plan plan);
        public Task DeletePlan(Plan plan);
        public Task<Plan> SavePlan(Plan plan, Guid creatorId);

        
    }

}
