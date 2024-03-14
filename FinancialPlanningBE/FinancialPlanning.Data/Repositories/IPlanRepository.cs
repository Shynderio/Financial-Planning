using FinancialPlanning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public  interface IPlanRepository
    {
        public Task<List<Plan>> GetAllPlans();
        public Task<List<Plan>> GetPlanByDepartId();
    }
}
