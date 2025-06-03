namespace BookService.Models
{
    public class BookingRequest
    {
        public string FlightId { get; set; }
        public string PassengerName { get; set; }
        public string PassengerEmail { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
