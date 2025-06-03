using Xunit;
using BookService.Controllers;
using BookService.Models;
using BookService.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace BookService.Tests
{
    public class BookServiceTests
    {
        private readonly BookingContext _context;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly BookingController _controller;

        public BookServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookingContext>()
                .UseInMemoryDatabase(databaseName: "TestBookingDb")
                .Options;

            _context = new BookingContext(options);
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfig = new Mock<IConfiguration>();
            _controller = new BookingController(_context, _mockHttpClientFactory.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 99, FlightId = "683f2391a801181540f7635e", PassengerFirstname = "Max",  PassengerLastname = "Mustermann" },
                new Booking { Id = 89, FlightId = "683f2391a801181540f7635e", PassengerFirstname = "John", PassengerLastname = "Doe" }
            };

            await _context.Bookings.AddRangeAsync(bookings);
            await _context.SaveChangesAsync();

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = Assert.IsType<ActionResult<IEnumerable<Booking>>>(actionResult);
            var bookingList = Assert.IsAssignableFrom<IEnumerable<Booking>>(result.Value);
            Assert.Equal(2, ((List<Booking>)bookingList).Count);
        }

        [Fact]
        public async Task GetById_ExistingBooking_ReturnsBooking()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                FlightId = "FL001",
                PassengerFirstname = "Max",
                PassengerLastname = "Mustermann",
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            // Act
            var actionResult = await _controller.GetById(1);

            // Assert
            var result = Assert.IsType<ActionResult<Booking>>(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooking = Assert.IsType<Booking>(okResult.Value);
            Assert.Equal(booking.Id, returnedBooking.Id);
            Assert.Equal(booking.PassengerFirstname, returnedBooking.PassengerFirstname);
            Assert.Equal(booking.PassengerLastname, returnedBooking.PassengerLastname);

    }
        [Fact]
        public async Task GetById_NonExistingBooking_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999;

            // Act
            var actionResult = await _controller.GetById(nonExistingId);

            // Assert
            var result = Assert.IsType<ActionResult<Booking>>(actionResult);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidBooking_ReturnsCreatedBooking()
        {
            // Arrange
            var booking = new Booking
            {
                FlightId = "683f2391a801181540f7635e",
                PassengerFirstname = "Max",
                PassengerLastname = "Mustermann",
            };

            var mockHttpClient = new Mock<HttpClient>();
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            _mockHttpClientFactory.Setup(f => f.CreateClient())
                .Returns(mockHttpClient.Object);

            _mockConfig.Setup(c => c["FlightServiceUrl"])
                .Returns("http://flightservice:80");

            // Act
            var result = await _controller.Create(booking);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedBooking = Assert.IsType<Booking>(createdAtActionResult.Value);
            Assert.Equal(booking.PassengerFirstname, returnedBooking.PassengerFirstname);
            Assert.Equal(booking.PassengerLastname, returnedBooking.PassengerLastname);
            Assert.Equal(booking.FlightId, returnedBooking.FlightId);
        }
    }
} 