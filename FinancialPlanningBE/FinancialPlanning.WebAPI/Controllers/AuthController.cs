using FinancialPlanning.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialPlanning.Data.Entities;
using AutoMapper;
using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

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
                return BadRequest();
            }
            //mapper loginmodel to user
            var user = mapper.Map<User>(model);

                //Check acc and create token
                var token = await authService.LoginAsync(user);

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

        [HttpGet]
        [Authorize(Roles = "FinancialStaff, Accountant")]
        public async Task<IActionResult> Test()
        {
            IActionResult respone = Unauthorized();
            respone = Ok(new { mess = "ok" });

            return Ok(respone);
        }



    }
}
