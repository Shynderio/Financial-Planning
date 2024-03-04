using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinancialPlanningDAL.Entities;

using FinancialPlanningDAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinancialPlanningBAL
{
    public class AuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration) {
        this.authRepository = authRepository;
            this.configuration = configuration;
        }    

        public async Task<string> LoginAsync(User userMapper)
        {
         

            var user = await authRepository.IsValidUser(userMapper.Email, userMapper.Password);
            if (user != null)
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRole = await authRepository.GetRoleUser(user.Email);
                authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));


                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));


                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);

            }

            return string.Empty;


           
        }
    }
}
