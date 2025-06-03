namespace BookService.Models;

public class Booking
{
    public int Id { get; set; }
    public string FlightId { get; set; } = string.Empty;
    public int PassengerId { get; set; }
    public string PassengerFirstname { get; set; } = string.Empty;
    public string PassengerLastname { get; set; } = string.Empty;
    public int TicketCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
