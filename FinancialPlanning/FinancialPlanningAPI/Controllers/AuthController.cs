﻿using FinancialPlanning.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialPlanningDAL.Entities;
using FinancialPlanningBAL;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly IMapper mapper;
        public AuthController(AuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginModel model)
        {
            IActionResult respone = Unauthorized();

            //InValid Model
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid username or password" });
            }
            //mapper loginmodel to user
            var user = mapper.Map<User>(model);

                //Check acc and create token
                var token = await authService.LoginAsync(user);

                //Invalid account and returned emtry
                if (string.IsNullOrEmpty(token))
                {
                    respone = BadRequest(new { message = "Invalid username or password" });

                }
                else
                {
                    respone = Ok(new { token = token, message = "Login successful" });
                }


                return Ok(respone);
          
           

        }
       

    }
}
