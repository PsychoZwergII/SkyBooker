using Xunit;
using AuthService.Controllers;
using AuthService.Models;
using AuthService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;

namespace AuthService.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthServiceTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockLogger.Object, _mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "password123"
            };

            var expectedResponse = new AuthResponse
            {
                Token = "jwt-token-123",
                Username = "testuser",
                Success = true,
                Message = "Login successful"
            };

            _mockAuthService.Setup(service => service.LoginAsync(loginRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await _controller.Login(loginRequest);
            var result = (actionResult.Result as OkObjectResult)?.Value as AuthResponse;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Token, result.Token);
            Assert.Equal(expectedResponse.Username, result.Username);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "wronguser",
                Password = "wrongpass"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Invalid username or password"
            };

            _mockAuthService.Setup(service => service.LoginAsync(loginRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await _controller.Login(loginRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsSuccess()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "newuser@example.com"
            };

            var expectedResponse = new AuthResponse
            {
                Success = true,
                Token = "new-jwt-token-123",
                Username = "newuser",
                Message = "Registration successful"
            };

            _mockAuthService.Setup(service => service.RegisterAsync(registerRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await _controller.Register(registerRequest);
            var result = (actionResult.Result as OkObjectResult)?.Value as AuthResponse;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Token, result.Token);
            Assert.Equal(expectedResponse.Username, result.Username);
        }

        [Fact]
        public async Task Register_DuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Username = "existinguser",
                Password = "password123",
                Email = "existing@example.com"
            };

            var expectedResponse = new AuthResponse
            {
                Success = false,
                Message = "Username already exists"
            };

            _mockAuthService.Setup(service => service.RegisterAsync(registerRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await _controller.Register(registerRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task ValidateToken_ValidToken_ReturnsTrue()
        {
            // Arrange
            var token = "valid-jwt-token-123";
            
            _mockAuthService.Setup(service => service.ValidateTokenAsync(token))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _controller.ValidateToken(token);
            var okResult = Assert.IsType<ActionResult<bool>>(actionResult);
            var result = Assert.IsType<bool>((okResult.Result as OkObjectResult)?.Value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateToken_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var token = "invalid-token";
            
            _mockAuthService.Setup(service => service.ValidateTokenAsync(token))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _controller.ValidateToken(token);
            var okResult = Assert.IsType<ActionResult<bool>>(actionResult);
            var result = Assert.IsType<bool>((okResult.Result as OkObjectResult)?.Value);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateToken_EmptyToken_ReturnsFalse()
        {
            // Arrange
            var token = "";
            
            _mockAuthService.Setup(service => service.ValidateTokenAsync(token))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _controller.ValidateToken(token);
            var okResult = Assert.IsType<ActionResult<bool>>(actionResult);
            var result = Assert.IsType<bool>((okResult.Result as OkObjectResult)?.Value);

            // Assert
            Assert.False(result);
        }
    }
} 