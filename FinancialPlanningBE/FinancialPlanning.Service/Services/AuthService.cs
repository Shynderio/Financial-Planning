using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FinancialPlanning.Service.DTOs;
using static System.Security.Claims.ClaimTypes;

namespace FinancialPlanning.Service.Services
{
    public class AuthService
    {
        private readonly EmailService _emailService;
        private readonly IAuthRepository _authRepository;
        private readonly IDepartmentRepository _depRepository;

        private readonly IConfiguration _configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, EmailService emailService, IDepartmentRepository depRepository)
        {
            this._authRepository = authRepository;
            this._configuration = configuration;
            _emailService = emailService;
            this._depRepository = depRepository;
        }

        private async Task ValidateToken(string email, string token)
        {
            var tokenFromDb = await _authRepository.GetToken(email) ?? throw new Exception("Token not found");
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
            await _authRepository.ResetPassword(user);
        }

        public string GenerateToken(string email)
        {
            var jwtService = new JwtService(_configuration["JWT:Secret"]!, _configuration["JWT:ValidIssuer"]!);
            var token = jwtService.GenerateToken(email);
            _authRepository.SetToken(email, token);
            return token;
        }

        public void SendResetPasswordEmail(string userEmail, string resetToken)
        {
            var resetUrl = $"http://localhost:4200/reset-password?token={resetToken}";

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
            return await _authRepository.IsUser(email);
        }

        public async Task<string> LoginAsync(User userMapper)
        {

            //Check email and pass
            var user = await _authRepository.IsValidUser(userMapper.Email, userMapper.Password);

            if (user == null) return string.Empty;
            //add email to claim
            var authClaims = new List<Claim>
            {
                new(Email, user.Email),
                new("userId", user.Id.ToString()),
                new("username",user.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //Get role of user
            var userRole = await _authRepository.GetRoleUser(user.Email);
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            //Add departmentName claim
            var departmentname = await _depRepository.GetDepartmentNameByUser(user);
            authClaims.Add(new Claim("departmentName", departmentname));

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
              
            //Create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddDays(30),
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

    }
}
