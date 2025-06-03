using Xunit;
using FlightService.Controllers;
using FlightService.Models;
using FlightService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FlightService.Tests
{
    public class FlightServiceTests
    {
        private readonly Mock<IFlightService> _mockFlightService;
        private readonly Mock<ILogger<FlightController>> _mockLogger;
        private readonly FlightController _controller;

        public FlightServiceTests()
        {
            _mockFlightService = new Mock<IFlightService>();
            _mockLogger = new Mock<ILogger<FlightController>>();
            _controller = new FlightController(_mockLogger.Object, _mockFlightService.Object);
        }

        [Fact]
        public async Task GetFlights_ReturnsFlightList()
        {
            // Arrange
            var expectedFlights = new List<Flight>
            {
                new Flight { Id = "1", FlightNumber = "FL001", DepartureCity = "Zürich", ArrivalCity = "London" },
                new Flight { Id = "2", FlightNumber = "FL002", DepartureCity = "London", ArrivalCity = "Paris" }
            };

            _mockFlightService.Setup(s => s.GetFlightsAsync())
                .ReturnsAsync(expectedFlights);

            // Act
            var result = await _controller.GetFlights();

            // Assert
            Assert.Equal(expectedFlights, result);
        }

        [Fact]
        public async Task GetFlightById_ReturnsCorrectFlight()
        {
            // Arrange
            var expectedFlight = new Flight
            {
                Id = "1",
                FlightNumber = "FL001",
                DepartureCity = "Zürich",
                ArrivalCity = "London"
            };

            _mockFlightService.Setup(s => s.GetFlightByIdAsync("1"))
                .ReturnsAsync(expectedFlight);

            // Act
            var result = await _controller.GetFlightById("1");

            // Assert
            Assert.Equal(expectedFlight, result);
        }

        [Fact]
        public async Task GetFlightById_NonExistentId_ReturnsNull()
        {
            // Arrange
            _mockFlightService.Setup(s => s.GetFlightByIdAsync("999"))
                .ReturnsAsync((Flight)null);

            // Act
            var result = await _controller.GetFlightById("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchFlights_ReturnsMatchingFlights()
        {
            // Arrange
            var criteria = new FlightSearchCriteria
            {
                DepartureCity = "Zürich",
                ArrivalCity = "London",
                DepartureDate = DateTime.Today,
                MaxPrice = 500,
                NumberOfPassengers = 2
            };

            var expectedFlights = new List<Flight>
            {
                new Flight
                {
                    Id = "1",
                    FlightNumber = "FL001",
                    DepartureCity = "Zürich",
                    ArrivalCity = "London",
                    DepartureTime = DateTime.Today.AddHours(10),
                    Price = 400,
                    AvailableSeats = 5
                }
            };

            _mockFlightService.Setup(s => s.SearchFlightsAsync(criteria))
                .ReturnsAsync(expectedFlights);

            // Act
            var result = await _controller.SearchFlights(criteria);

            // Assert
            Assert.Single(result);
            var flight = result.First();
            Assert.Equal("Zürich", flight.DepartureCity);
            Assert.Equal("London", flight.ArrivalCity);
            Assert.Equal(DateTime.Today.AddHours(10), flight.DepartureTime);
        }

        [Fact]
        public async Task SearchFlights_NoMatchingFlights_ReturnsEmptyList()
        {
            // Arrange
            var criteria = new FlightSearchCriteria
            {
                DepartureCity = "Paris",
                ArrivalCity = "Tokyo",
                DepartureDate = DateTime.Today
            };

            _mockFlightService.Setup(s => s.SearchFlightsAsync(criteria))
                .ReturnsAsync(new List<Flight>());

            // Act
            var result = await _controller.SearchFlights(criteria);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchFlights_WithMaxPrice_ReturnsOnlyFlightsWithinBudget()
        {
            // Arrange
            var criteria = new FlightSearchCriteria
            {
                MaxPrice = 300
            };

            var expectedFlights = new List<Flight>
            {
                new Flight { Id = "1", FlightNumber = "FL001", Price = 250 },
                new Flight { Id = "2", FlightNumber = "FL002", Price = 280 }
            };

            _mockFlightService.Setup(s => s.SearchFlightsAsync(criteria))
                .ReturnsAsync(expectedFlights);

            // Act
            var result = await _controller.SearchFlights(criteria);

            // Assert
            Assert.All(result, flight => Assert.True(flight.Price <= 300));
        }
    }
} 