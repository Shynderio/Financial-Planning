using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;

namespace FinancialPlanning.Service.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //Get all user
        public async Task<List<User>> GetAllUsers()
        {
            var result = await _userRepository.GetAllUsers();
            return result;
        }
        //Get user by Id
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            return user;

        }
        //Update user
        public async Task UpdateUser(Guid id, User user)
        {
            await _userRepository.UpdateUser(id, user);
        }
        //Add new user
        public async Task AddNewUser(User user)
        {
            await _userRepository.AddNewUser(user);
        }
        //Update user status
        public async Task UpdateUserStatus(Guid id, int status)
        {
            await _userRepository.UpdateUserStatus(id, status);
        }
        //Delete user
        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteUser(id);
        }
    }
}
