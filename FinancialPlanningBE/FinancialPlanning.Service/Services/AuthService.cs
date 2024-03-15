using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FinancialPlanning.Service.DTOs;

namespace FinancialPlanning.Service.Services
{
    public class AuthService
    {
        private readonly EmailService _emailService;
        private readonly IAuthRepository authRepository;
        private readonly IDepartmentRepository depRepository;

        private readonly IConfiguration configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, EmailService emailService, IDepartmentRepository depRepository)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
            _emailService = emailService;
            this.depRepository = depRepository;
        }

        public async Task ValidateToken(string email, string token)
        {
            var tokenFromDb = await authRepository.GetToken(email) ?? throw new Exception("Token not found");
            if (tokenFromDb != token)
            {
                throw new Exception("Invalid token");
            }
            
            if (JwtService.IsTokenExpired(token))
            {
                throw new Exception("Token expired");
            }
        }

        public async Task ResetPassword(User user)
        {
            // Validate token
            var token = user.Token ?? throw new Exception("Token not found");
            await ValidateToken(user.Email, token);
            user.Token = null;
            // If token is valid, proceed with password reset
            await authRepository.ResetPassword(user);
        }

        public string GenerateToken(string email)
        {
            var JwtService = new JwtService(configuration["JWT:Secret"]!, configuration["JWT:ValidIssuer"]!);
            string token = JwtService.GenerateToken(email);
            authRepository.SetToken(email, token);
            return token;
        }

        public void SendResetPasswordEmail(string userEmail, string resetToken)
        {
            var resetUrl = $"http://localhost:4200/forgotpassword?token={resetToken}";

            var email = new EmailDTO
            {
                To = userEmail,
                Subject = "Password Reset",
                Body = $"Click <a href=\"{resetUrl}\">here</a> to reset your password."
            };

            _emailService.SendEmail(email);
        }

        public async Task<bool> IsUser(string email)
        {
            return await authRepository.IsUser(email);
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

                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
              
                //Create token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = configuration["JWT:ValidIssuer"],
                    Audience = configuration["JWT:ValidAudience"],
                    Expires = DateTime.UtcNow.AddDays(7),
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
