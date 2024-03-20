using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class UserService
    {
<<<<<<< HEAD
        private readonly IUserRepository _userrepository;
        private readonly IDepartmentRepository _departmentrepository;
=======
        private readonly IUserRepository _userRepository;
>>>>>>> e702b2c530a80aa963c1265da63f154b79eddf2c

        public UserService(IUserRepository userRepository , IDepartmentRepository departmentRepository)
        {
<<<<<<< HEAD
            _userrepository = userRepository;
            _departmentrepository = departmentRepository;
=======
            _userRepository = userRepository;
>>>>>>> e702b2c530a80aa963c1265da63f154b79eddf2c
        }
        //Get all user
        public async Task<List<User>> GetAllUsers()
        {
            var result = await _userrepository.GetAllUsers();
            var result = await _userRepository.GetAllUsers();
            return result;
        }

        //Get all department
        public async Task<List<Department>> GetAllDepartment()
        {
            return await _departmentrepository.GetAllDepartment();
        }
        //Get user by Id
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userrepository.GetUserById(id);
            var user = await _userRepository.GetUserById(id);
            return user;

        }
        //Update user
        public async Task UpdateUser(Guid id, User user)
        {
            await _userrepository.UpdateUser(id, user);
            await _userRepository.UpdateUser(id, user);
        }
        //Add new user
        public async Task AddNewUser(User user)
        {
            await _userrepository.AddNewUser(user);
            await _userRepository.AddNewUser(user);
        }
        //Update user status
        public async Task updateUserStatus(Guid id, int status)
        public async Task UpdateUserStatus(Guid id, int status)
        {
            await _userrepository.UpdateUserStatus(id, status);
            await _userRepository.UpdateUserStatus(id, status);
        }
        //Delete user
        public async Task DeleteUser(Guid id)
        {
            await _userrepository.DeleteUser(id);
            await _userRepository.DeleteUser(id);
        }
    }
}
