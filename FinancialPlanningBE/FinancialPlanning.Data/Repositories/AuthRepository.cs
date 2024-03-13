using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanning.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<User?> IsValidUser(string email, string password)
        {
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user;
        }

        public async Task<string> GetRoleUser(string email)
        {
            if (context.Users != null)
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
            }

            return "";
        }

      

    }
}
