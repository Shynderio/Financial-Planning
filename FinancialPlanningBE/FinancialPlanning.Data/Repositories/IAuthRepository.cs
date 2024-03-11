using FinancialPlanning.Data.Entities;

namespace FinancialPlanning.Data.Repositories
{
    public interface IAuthRepository
    {
        public Task<User> IsValidUser(string email, string password);
        public  Task<String> GetRoleUser(string email);
    }
}
