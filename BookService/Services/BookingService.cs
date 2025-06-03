using BookService.Models;

namespace BookService.Services
{
    public class BookingService
    {
        private static readonly List<Booking> _bookings = new();
        private static int _nextId = 1;

        public Task<List<Booking>> GetAllAsync()
        {
            return Task.FromResult(_bookings);
        }

        public Task<Booking?> GetByIdAsync(int id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            return Task.FromResult(booking);
        }

        public Task AddAsync(Booking booking)
        {
            booking.Id = _nextId++;
            _bookings.Add(booking);
            return Task.CompletedTask;
        }
    }
}
