using FinancialPlanningBAL;
using FinancialPlanningDAL.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinancialPlanning.Data.Repositories;

namespace Test.UnitTesting.Service
{
    public class AuthServiceTest
    {
        //test 1 login ok 
        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsToken()
        {
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var user = new User
            {
                Email = "email1@example.com",
                Password = "password1"
            };

            var userRole = "Admin";
           

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync(userRole);

            mockConfiguration.SetupGet(config => config["JWT:Secret"]).Returns("ThisIsASecretKeyThatYouWillNeverKnow!2#4%6&8(0");
            mockConfiguration.SetupGet(config => config["JWT:ValidIssuer"]).Returns("https://localhost:7270");
            mockConfiguration.SetupGet(config => config["JWT:ValidAudience"]).Returns("User");

            // Act
            var token = await authService.LoginAsync(user);

            // Assert
            Assert.NotEmpty(token);
        }
        //test 2 login invalid email or pass
        [Fact]
        public async Task LoginAsync_InvalidUser_ReturnsEmptyString()
        {
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var user = new User
            {
                Email = "email2@example",
                Password = "password2"
            };

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync((User)null);

            // Act
            var token = await authService.LoginAsync(user);

            // Assert
            Assert.Empty(token);
        }

        //test 3 login Invalid Configuration
        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsTokenWithUserRoleClaim()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var user = new User
            {
                Email = "email1@example.com",
                Password = "password1"
            };

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync("Admin");

            mockConfiguration.SetupGet(config => config["JWT:Secret"]).Returns("ThisIsASecretKeyThatYouWillNeverKnow!2#4%6&8(0");
            mockConfiguration.SetupGet(config => config["JWT:ValidIssuer"]).Returns("https://localhost:7270");
            mockConfiguration.SetupGet(config => config["JWT:ValidAudience"]).Returns("User");

            // Act
            var token = await authService.LoginAsync(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            Assert.Contains(decodedToken.Claims, c => c.Type == "role" && c.Value == "Admin");

        }
        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsTokenWithExpirationClaim()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var user = new User
            {
                Email = "test@example.com",
                Password = "password"
            };

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync("UserRole");

            mockConfiguration.SetupGet(config => config["JWT:Secret"]).Returns("your_secret_key");
            mockConfiguration.SetupGet(config => config["JWT:ValidIssuer"]).Returns("valid_issuer");
            mockConfiguration.SetupGet(config => config["JWT:ValidAudience"]).Returns("valid_audience");

            // Act
            var token = await authService.LoginAsync(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            Assert.True(decodedToken.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsTokenWithValidJtiClaim()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var user = new User
            {
                Email = "test@example.com",
                Password = "password"
            };

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync("User");

            mockConfiguration.SetupGet(config => config["JWT:Secret"]).Returns("your_secret_key");
            mockConfiguration.SetupGet(config => config["JWT:ValidIssuer"]).Returns("valid_issuer");
            mockConfiguration.SetupGet(config => config["JWT:ValidAudience"]).Returns("valid_audience");

            // Act
            var token = await authService.LoginAsync(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            Assert.NotEmpty(decodedToken.Id);
        }

        // Add other test cases here...


    }
}
