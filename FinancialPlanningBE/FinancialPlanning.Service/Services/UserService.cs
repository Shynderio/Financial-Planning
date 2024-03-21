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

        private readonly IUserRepository _userrepository;
        private readonly IDepartmentRepository _departmentrepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPositionRepository _positionRepository;

        public UserService(IUserRepository userRepository , IDepartmentRepository departmentRepository, IRoleRepository roleRepository, IPositionRepository positionRepository)
        {

            _userrepository = userRepository;
            _departmentrepository = departmentRepository;
            _roleRepository = roleRepository;
            _positionRepository = positionRepository;

        }
        //Get all user
        public async Task<List<User>> GetAllUsers()
        {
            var result = await _userrepository.GetAllUsers();
            return result;
        }

        //Get all department
        public async Task<List<Department>> GetAllDepartment()
        {
            return await _departmentrepository.GetAllDepartment();
        }
        //Get all position
        public async Task<List<Position>> GetAllPositions()
        {
            return await _positionRepository.GetAllPositions();
        }
        //Get all role
        public async Task<List<Role>> GetAllRoles()
        {
            return await _roleRepository.GetAllRoles();
        }
        //Get user by Id
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userrepository.GetUserById(id);
            return user;

        }
        //Update user
        public async Task UpdateUser(Guid id, User user)
        {
            await _userrepository.UpdateUser(id, user);
        }
        //Add new user
        public async Task AddNewUser(User user)
        {
            await _userrepository.AddNewUser(user);
        }
        //Update user status
        public async Task UpdateUserStatus(Guid id, int status)
        {
            await _userrepository.UpdateUserStatus(id, status);
        }
        //Delete user
        public async Task DeleteUser(Guid id)
        {
            await _userrepository.DeleteUser(id);
        }
    }
}
