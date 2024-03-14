﻿using System.IdentityModel.Tokens.Jwt;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Test.UnitTesting.Service.Services
{
    public class AuthServiceTest
    {
        //test 1 login ok 
        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsToken()
        {
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
<<<<<<< HEAD
            var mockDepartRepository = new Mock<IDepartmentRepository>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockDepartRepository.Object);
=======
            var mockEmailService = new Mock<EmailService>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockEmailService.Object);
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4

            var user = new User
            {
                Email = "fianci@email.com",
                Password = "123"
            };

            var userRole = "Admin";
            var departmentName = "IT";


            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync(userRole);
            mockDepartRepository.Setup(repo => repo.GetDepartmentNameByUser(user))
                .ReturnsAsync(departmentName);

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
<<<<<<< HEAD
            var mockDepartRepository = new Mock<IDepartmentRepository>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockDepartRepository.Object);
=======
             var mockEmailService = new Mock<EmailService>();

            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockEmailService.Object);
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4

            var user = new User
            {
                Email = "fianci@email.com",
                Password = "123123"
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
<<<<<<< HEAD
            var mockDepartRepository = new Mock<IDepartmentRepository>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockDepartRepository.Object);
=======
             var mockEmailService = new Mock<EmailService>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockEmailService.Object);
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4

            var user = new User
            {
                Email = "fianci@email.com",
                Password = "123"
            };
            var departmentName = "IT";

            mockAuthRepository.Setup(repo => repo.IsValidUser(user.Email, user.Password))
                .ReturnsAsync(new User { Email = user.Email });

            mockAuthRepository.Setup(repo => repo.GetRoleUser(user.Email))
                .ReturnsAsync("Admin");
            mockDepartRepository.Setup(repo => repo.GetDepartmentNameByUser(user))
               .ReturnsAsync(departmentName);
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
<<<<<<< HEAD
=======
      
        // Add other test cases here...


>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4
    }
}