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

        public async Task<string> ValidateToken(string token)
        {
            var JwtService = new JwtService(configuration["JWT:Secret"]!, configuration["JWT:ValidIssuer"]!);

            if (JwtService.IsTokenExpired(token))
            {
                throw new Exception("Token expired");
            }
            var principal = JwtService.GetPrincipal(token) ?? throw new Exception("Invalid token");
            var emailClaim = principal.FindFirst(ClaimTypes.Email) ?? throw new Exception("Email claim not found in token");

            var email = emailClaim.Value;

            var tokenFromDb = await authRepository.GetToken(email) ?? throw new Exception("Token not found");

            // var tokenFromDb = await authRepository.GetToken(email) ?? throw new Exception("Token not found");
            if (tokenFromDb != token)
            {
                throw new Exception("Invalid token");
            }

            return email;
        }

        public async Task ResetPassword(User user)
        {
            // Validate token
            var token = user.Token ?? throw new Exception("Token not found");
            try {
                var email = await ValidateToken(token);
                user.Email = email;
                // If token is valid, proceed with password reset
                await authRepository.ResetPassword(user);
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
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

            var email = new EmailDto
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
                    new Claim("userId", user.Id.ToString()),
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
                    Expires = DateTime.UtcNow.AddDays(30),
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
