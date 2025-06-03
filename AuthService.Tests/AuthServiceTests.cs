using Xunit;
using AuthService.Controllers;
using AuthService.Models;
using AuthService.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;

namespace AuthService.Tests
{
    public class AuthServiceTests
    {
        private readonly UserContext _context;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestAuthDb")
                .Options;

            _context = new UserContext(options);
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("your-256-bit-secret");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("your-issuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("your-audience");
            _controller = new AuthController(_context, _mockConfig.Object);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Password = "password123",
                Email = "test@example.com"
            };

            // Act
            var result = await _controller.Register(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created", okResult.Value);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Password = password,
                Email = "test@example.com"
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value;
            Assert.NotNull(response.token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var username = "wronguser";
            var password = "wrongpass";

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
} 