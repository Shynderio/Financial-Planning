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
<<<<<<< HEAD
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
=======
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email && u.Password == password) ?? throw new Exception("Invalid username or password");
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4

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

<<<<<<< HEAD
      
=======
        public async Task ResetPassword(User user)
        {
            var userToUpdate = await context.Users!.SingleOrDefaultAsync(u => u.Email == user.Email) ?? throw new Exception("User not found");
            userToUpdate.Password = user.Password;
            userToUpdate.Token = null;
            await context.SaveChangesAsync();
        }
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4

        public async Task<bool> IsUser(string email)
        {
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email);
            return user != null;
        }

        public async Task SetToken(string email, string token)
        {
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.Token = token;
                await context.SaveChangesAsync();
            }
        }

        public async Task<string> GetToken(string email)
        {
            var user = await context.Users!.SingleOrDefaultAsync(u => u.Email == email) ?? throw new Exception("User not found");
            return user.Token!;
        }
    }
}
