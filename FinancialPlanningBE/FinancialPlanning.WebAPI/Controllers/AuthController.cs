﻿using Microsoft.AspNetCore.Mvc;
using FinancialPlanning.Data.Entities;
using AutoMapper;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using FinancialPlanning.WebAPI.Models.User;
using System.Threading.Tasks;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginModel model)
        {
            IActionResult respone;

            //InValid Model
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //mapper loginmodel to user
            var user = _mapper.Map<User>(model);

                //Check acc and create token
                var token = await _authService.LoginAsync(user);

                //Invalid account and returned emtry
                if (string.IsNullOrEmpty(token))
                {
                    respone = BadRequest();
                }
                else
                {
                    respone = Ok(new { token = token });
                }

                return Ok(respone);
        }
        
       
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email cannot be empty" });

            var isUser = await _authService.IsUser(email);
            
            if (isUser)
            {
                string token = _authService.GenerateToken(email);
                
                _authService.SendResetPasswordEmail(email, token);
                return Ok(new { message = "Email sent" });
            }
            else
            {
                return BadRequest(new { message = "Email not found" });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            // Validate model
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid password reset request" });

            // Check for null values in model properties
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Token))
                return BadRequest(new { message = "Email, password, and token cannot be empty" });

            try
            {
                User user = _mapper.Map<User>(model);
                // Attempt to reset password
                await _authService.ResetPassword(user);
                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                // Handle potential errors
                return StatusCode(500, new { message = ex });
            }
        }
    }
}
