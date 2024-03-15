using FinancialPlanning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancialPlanning.Data.Repositories
{

    //Interface for managing financial plans.
    public interface IPlanRepository
    {

        public Task<List<Plan>> GetAllPlans();
        public Task<List<Plan>> GetPlanByDepartId();
        //------------

        //Retrieve a list of financial plans based on role and department.

        public Task<List<Plan>> ListPlans(string role, string department);


        //View details of a plan based on the file.

        public Task<Plan> ViewPlan(string file);


        //Import a new financial plan.

        public Task<bool> ImportPlan(string uid, Guid termId, string planName, string file);


        //Re-upload an existing financial plan.

        public Task<bool> ReupPlan(string uid, Guid termId, string planName, string file);


        //Retrieve details of a financial plan based on term and department.

        public Task<Plan> GetPlanDetails(Guid termId, string department);


        //Retrieve a list of financial plans based on term and department.

        public Task<List<Plan>> GetPlans(Guid termId, string department);


        //Submit a financial plan for approval.

        public Task<bool> SubmitPlan(Guid termId, string planName, string departmentOrUid);


        //Approve a financial plan.

        public Task<bool> Approve(Guid termId, string planName, string departmentOrUid, string file);


        //Export a financial plan to a file.

        public Task<bool> ExportPlan(string file);
    }

}
