using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Service.Services;
using FinancialPlanning.WebAPI.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMapper mapper, UserService userService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly UserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        //Get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                //Map user to usermodel
                var userListModels = users.Select(u => _mapper.Map<UserModel>(u)).ToList();

                var departmentList = await _userService.GetAllDepartment();
                var positionList = await _userService.GetAllPositions();
                var roleList = await _userService.GetAllRoles();

                var result = new
                {
                    users = userListModels,
                    departments = departmentList,
                    roles = roleList

                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
          
        }
        //Get user by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel == null ? NotFound() : Ok(userModel);
        }

        //Add new user
        [HttpPost]
        public async Task<IActionResult> AddNewUser(AddUserModel userModel)
        {

            var user = _mapper.Map<User>(userModel);
            await _userService.AddNewUser(user);
            return Ok(user);
        }

        // Update User
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, AddUserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            user.Id = id;
            await _userService.UpdateUser(id, user);
            return Ok(new { message = $"User with id {id} updated successfully!" });
        }
        // Update status User
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid id, int status)
        {
            try
            {
                await _userService.UpdateUserStatus(id, status);
                return Ok(new { message = $"User with id {id} updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error updating user with id {id}: {ex.Message}" });
            }
        }
        //Delete user
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok(new { message = $"User with id {id} removed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error delete user with id {id}: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("AllDepartments")] 
        public async Task<ActionResult<List<Department>>> GetAllDepartments()
        {
            try
            {
                var departments = await _userService.GetAllDepartment();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }


}
