using Microsoft.AspNetCore.Mvc;
using FinancialPlanning.Data.Entities;
using AutoMapper;
using FinancialPlanning.Service.Services;
using FinancialPlanning.WebAPI.Models.User;
using Microsoft.AspNetCore.Http;

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
            IActionResult response;

            //InValid Model
            if (!ModelState.IsValid)
            {
                response = BadRequest();
            }
            //mapper loginmodel to user
            var user = _mapper.Map<User>(model);

            //Check acc and create token
            var token = await _authService.LoginAsync(user);

            //Invalid account and returned emtry
            if (string.IsNullOrEmpty(token))
            {
                response = BadRequest(new { message = "Invalid email or password" });
            }
            else
            {
                response = Ok(new { token });
            }

            return Ok(response);
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email cannot be empty" });

            var isUser = await _authService.IsUser(email);

            if (!isUser) return NotFound(new { message = "Email not found" });
            var token = _authService.GenerateToken(email);

            _authService.SendResetPasswordEmail(email, token);
            return Ok(new { message = "Email sent" });

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Invalid password reset request" });

                // Check for null values in model properties
                if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Token))
                    return BadRequest(new { message = "Password and token cannot be empty" });

                User user = _mapper.Map<User>(model);
                // Attempt to reset password
                await _authService.ResetPassword(user);

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                // Handle potential errors
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
