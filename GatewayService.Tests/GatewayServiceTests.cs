using Xunit;
using GatewayService.Controllers;
using GatewayService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace GatewayService.Tests
{
    public class GatewayServiceTests
    {
        private readonly Mock<IGatewayService> _mockGatewayService;
        private readonly Mock<ILogger<GatewayController>> _mockLogger;
        private readonly GatewayController _controller;

        public GatewayServiceTests()
        {
            _mockGatewayService = new Mock<IGatewayService>();
            _mockLogger = new Mock<ILogger<GatewayController>>();
            _controller = new GatewayController(_mockLogger.Object, _mockGatewayService.Object);
            
            // Setup default HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task ForwardGet_ValidRequest_ReturnsResponse()
        {
            // Arrange
            var service = "FlightService";
            var path = "api/flights";
            var responseContent = "[{\"id\":\"1\",\"flightNumber\":\"FL001\"}]";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockGatewayService.Setup(s => s.ForwardRequestAsync(
                service, path, HttpMethod.Get, null))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ForwardGet(service, path);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(responseContent, okResult.Value);
        }

        [Fact]
        public async Task ForwardGet_ServiceUnavailable_ReturnsServiceUnavailable()
        {
            // Arrange
            var service = "FlightService";
            var path = "api/flights";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent("Service unavailable")
            };

            _mockGatewayService.Setup(s => s.ForwardRequestAsync(
                service, path, HttpMethod.Get, null))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ForwardGet(service, path);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);
        }

        [Fact]
        public async Task ForwardPost_ValidRequest_ReturnsResponse()
        {
            // Arrange
            var service = "BookService";
            var path = "api/bookings";
            var requestContent = "{\"flightId\":\"1\",\"passengerName\":\"Test\"}";
            var responseContent = "{\"id\":\"1\",\"status\":\"confirmed\"}";
            
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));
            _controller.ControllerContext.HttpContext.Request.Body = stream;
            _controller.ControllerContext.HttpContext.Request.ContentLength = stream.Length;

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(responseContent)
            };

            _mockGatewayService.Setup(s => s.ForwardRequestAsync(
                service, path, HttpMethod.Post, requestContent))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ForwardPost(service, path);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, statusResult.StatusCode);
            Assert.Equal(responseContent, statusResult.Value);
        }

        [Fact]
        public async Task ForwardPost_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var service = "BookService";
            var path = "api/bookings";
            var requestContent = "{\"invalid\":\"request\"}";
            
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));
            _controller.ControllerContext.HttpContext.Request.Body = stream;
            _controller.ControllerContext.HttpContext.Request.ContentLength = stream.Length;

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Invalid request")
            };

            _mockGatewayService.Setup(s => s.ForwardRequestAsync(
                service, path, HttpMethod.Post, requestContent))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ForwardPost(service, path);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);
        }

        [Fact]
        public async Task ForwardGet_UnknownService_ReturnsNotFound()
        {
            // Arrange
            var service = "UnknownService";
            var path = "api/unknown";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Service not found")
            };

            _mockGatewayService.Setup(s => s.ForwardRequestAsync(
                service, path, HttpMethod.Get, null))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ForwardGet(service, path);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, statusResult.StatusCode);
        }
    }
} 