using Xunit;
using BookService.Controllers;
using BookService.Models;
using BookService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly Mock<ILogger<BookingController>> _mockLogger;
        private readonly BookingController _controller;

        public BookServiceTests()
        {
            _mockBookingService = new Mock<IBookingService>();
            _mockLogger = new Mock<ILogger<BookingController>>();
            _controller = new BookingController(_mockLogger.Object, _mockBookingService.Object);
        }

        [Fact]
        public async Task CreateBooking_ValidRequest_ReturnsNewBooking()
        {
            // Arrange
            var bookingRequest = new BookingRequest
            {
                FlightId = "1",
                PassengerName = "Max Mustermann",
                PassengerEmail = "max@example.com",
                NumberOfSeats = 2
            };

            var expectedBooking = new Booking
            {
                Id = "1",
                FlightId = bookingRequest.FlightId,
                PassengerName = bookingRequest.PassengerName,
                PassengerEmail = bookingRequest.PassengerEmail,
                NumberOfSeats = bookingRequest.NumberOfSeats,
                BookingStatus = BookingStatus.Confirmed,
                BookingDate = DateTime.Now
            };

            _mockBookingService.Setup(service => service.CreateBookingAsync(It.IsAny<BookingRequest>()))
                .ReturnsAsync(expectedBooking);

            // Act
            var result = await _controller.CreateBooking(bookingRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBooking.Id, result.Id);
            Assert.Equal(expectedBooking.PassengerName, result.PassengerName);
            Assert.Equal(expectedBooking.BookingStatus, result.BookingStatus);
        }

        [Fact]
        public async Task GetBooking_ExistingId_ReturnsBooking()
        {
            // Arrange
            var bookingId = "1";
            var expectedBooking = new Booking
            {
                Id = bookingId,
                FlightId = "FL123",
                PassengerName = "Max Mustermann",
                PassengerEmail = "max@example.com",
                NumberOfSeats = 2,
                BookingStatus = BookingStatus.Confirmed,
                BookingDate = DateTime.Now
            };

            _mockBookingService.Setup(service => service.GetBookingAsync(bookingId))
                .ReturnsAsync(expectedBooking);

            // Act
            var result = await _controller.GetBooking(bookingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBooking.Id, result.Id);
            Assert.Equal(expectedBooking.PassengerName, result.PassengerName);
        }

        [Fact]
        public async Task GetBooking_NonExistentId_ReturnsNull()
        {
            // Arrange
            var bookingId = "999";
            _mockBookingService.Setup(service => service.GetBookingAsync(bookingId))
                .ReturnsAsync((Booking)null);

            // Act
            var result = await _controller.GetBooking(bookingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CancelBooking_ExistingBooking_UpdatesStatus()
        {
            // Arrange
            var bookingId = "1";
            var expectedBooking = new Booking
            {
                Id = bookingId,
                FlightId = "FL123",
                PassengerName = "Max Mustermann",
                PassengerEmail = "max@example.com",
                NumberOfSeats = 2,
                BookingStatus = BookingStatus.Cancelled,
                BookingDate = DateTime.Now
            };

            _mockBookingService.Setup(service => service.CancelBookingAsync(bookingId))
                .ReturnsAsync(expectedBooking);

            // Act
            var result = await _controller.CancelBooking(bookingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(BookingStatus.Cancelled, result.BookingStatus);
        }

        [Fact]
        public async Task CancelBooking_NonExistentBooking_ReturnsNull()
        {
            // Arrange
            var bookingId = "999";
            _mockBookingService.Setup(service => service.CancelBookingAsync(bookingId))
                .ReturnsAsync((Booking)null);

            // Act
            var result = await _controller.CancelBooking(bookingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateBooking_InvalidRequest_ThrowsException()
        {
            // Arrange
            var invalidRequest = new BookingRequest
            {
                FlightId = "", // Ungültige FlightId
                PassengerName = "",
                PassengerEmail = "invalid-email",
                NumberOfSeats = 0
            };

            _mockBookingService.Setup(service => service.CreateBookingAsync(invalidRequest))
                .ThrowsAsync(new ArgumentException("Invalid booking request"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _controller.CreateBooking(invalidRequest));
        }
    }
} 