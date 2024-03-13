using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinancialPlanning.Service.Services
{
    public class AuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly IDepartmentRepository depRepository;

        private readonly IConfiguration configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IDepartmentRepository depRepository)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
            this.depRepository = depRepository;
        }

        public async Task<string> LoginAsync(User userMapper)
        {

            //Check email and pass
            var user = await authRepository.IsValidUser(userMapper.Email, userMapper.Password);
            
            if (user != null)
            {
                //add email to claim
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("username",user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                //Get role of user
                var userRole = await authRepository.GetRoleUser(user.Email);
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));

                //Add departmentName claim
                var departmentname = await depRepository.GetDepartmentNameByUser(user);
                authClaims.Add(new Claim("departmentName", departmentname));

                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
              
                //Create token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = configuration["JWT:ValidIssuer"],
                    Audience = configuration["JWT:ValidAudience"],
                    Expires = DateTime.UtcNow.AddMinutes(20),
                    Subject = new ClaimsIdentity(authClaims),
                    SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);

            }

            return string.Empty;
        }

       
    }
}
