using Xunit;
using FlightService.Controllers;
using FlightService.Models;
using FlightService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightService.Tests
{
    public class FlightServiceTests
    {
        private readonly Mock<IFlightService> _mockFlightService;
        private readonly FlightController _controller;

        public FlightServiceTests()
        {
            _mockFlightService = new Mock<IFlightService>();
            _controller = new FlightController(_mockFlightService.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllFlights()
        {
            // Arrange
            var expectedFlights = new List<Flight>
            {
                new Flight { Id = "FL001", Source = "Zürich", Destination = "London" },
                new Flight { Id = "FL002", Source = "London", Destination = "Paris" }
            };

            _mockFlightService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedFlights);

            // Act
            var actionResult = await _controller.Get();

            // Assert
            var result = Assert.IsType<ActionResult<List<Flight>>>(actionResult);
            var flights = Assert.IsType<List<Flight>>(result.Value);
            Assert.Equal(2, flights.Count);
        }

        [Fact]
        public async Task GetById_ExistingFlight_ReturnsFlight()
        {
            // Arrange
            var expectedFlight = new Flight
            {
                Id = "FL001",
                Source = "Zürich",
                Destination = "London"
            };

            _mockFlightService.Setup(s => s.GetByIdAsync("FL001"))
                .ReturnsAsync(expectedFlight);

            // Act
            var actionResult = await _controller.GetById("FL001");

            // Assert
            var result = Assert.IsType<ActionResult<Flight>>(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var flight = Assert.IsType<Flight>(okResult.Value);
            Assert.Equal(expectedFlight.Id, flight.Id);
            Assert.Equal(expectedFlight.Source, flight.Source);
        }

        [Fact]
        public async Task GetById_NonExistingFlight_ReturnsNotFound()
        {
            // Arrange
            _mockFlightService.Setup(s => s.GetByIdAsync("FL999"))
                .ReturnsAsync((Flight)null);

            // Act
            var actionResult = await _controller.GetById("FL999");

            // Assert
            var result = Assert.IsType<ActionResult<Flight>>(actionResult);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidFlight_ReturnsCreatedFlight()
        {
            // Arrange
            var flight = new Flight
            {
                Id = "FL003",
                Source = "Berlin",
                Destination = "Paris"
            };

            _mockFlightService.Setup(s => s.AddAsync(flight))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(flight);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedFlight = Assert.IsType<Flight>(createdAtActionResult.Value);
            Assert.Equal(flight.Id, returnedFlight.Id);
            Assert.Equal(flight.Source, returnedFlight.Source);
        }
    }
} 