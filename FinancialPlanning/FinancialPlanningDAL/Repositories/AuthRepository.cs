using FinancialPlanningDAL.Data;
using FinancialPlanningDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
    
        public AuthRepository(DataContext context)
        {
            this.context = context;
          
        }
        public async Task<User> IsValidUser(string email, string password)
        {
         
         var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
                return user;
         
          
          
        }
        public async Task<string> GetRoleUser(string email)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                var role = await context.Roles.SingleOrDefaultAsync(r => r.Id == user.RoleId);
                if (role != null)
                {
                    return role.RoleName;
                }
            }


            return "";

        }

        
    }
}
