using FinancialPlanningAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialPlanningDAL.Entities;
using FinancialPlanningBAL;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
namespace FinancialPlanningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly IMapper mapper;
        public AuthController(AuthService authService, IMapper mapper) {
        this.authService = authService;
            this.mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginModel model)
        {
            var user = mapper.Map<User>(model); 

            var result = await authService.LoginAsync(user);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Check()
        {
            return Ok();
        }

    }
}
