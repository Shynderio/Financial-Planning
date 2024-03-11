using FinancialPlanningDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Repositories
{
    public interface IAuthRepository
    {
        public Task<User> IsValidUser(string email, string password);
        public  Task<String> GetRoleUser(string email);
    }
}
