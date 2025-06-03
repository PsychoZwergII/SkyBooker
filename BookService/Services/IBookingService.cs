using BookService.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookService.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBooking(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<Booking?> GetBookingById(int id);
    }
} 