﻿using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Service.Services;
using FinancialPlanning.WebAPI.Models.Term;
using FinancialPlanning.WebAPI.Models.User;
using Microsoft.AspNetCore.Http;
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
            var users = await _userService.GetAllUsers();
            var userListModels = users.Select(u => _mapper.Map<UserModel>(u)).ToList();
            return Ok(userListModels);
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

    }


}
