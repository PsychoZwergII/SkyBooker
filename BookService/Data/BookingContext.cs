using Microsoft.EntityFrameworkCore;
using BookService.Models;

namespace BookService.Data;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

    public DbSet<Booking> Bookings => Set<Booking>();
}
