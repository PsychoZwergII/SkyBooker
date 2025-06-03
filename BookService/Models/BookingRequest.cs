namespace BookService.Models
{
    public class BookingRequest
    {
        public string UserId { get; set; }
        public string FlightId { get; set; }
        public string PassengerName { get; set; }  // z.B. "Leon Egli"
        public string PassengerEmail { get; set; }
        public int SeatsBooked { get; set; }
    }
}
