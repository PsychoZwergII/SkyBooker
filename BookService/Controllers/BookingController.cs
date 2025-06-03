using BookService.Models;
using BookService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BookingController(BookingService bookingService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _bookingService = bookingService;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Booking>> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        if (booking == null) return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] BookingRequest request)
    {
        // Schritt 1: Flugdaten prüfen
        var client = _httpClientFactory.CreateClient();
        var flightServiceUrl = _configuration["FlightServiceUrl"] ?? "http://localhost:5221";
        var flightResponse = await client.GetAsync($"{flightServiceUrl}/api/Flight/{request.FlightId}");

        if (!flightResponse.IsSuccessStatusCode)
            return BadRequest("Ungültige Flugnummer (FlightId nicht gefunden)");

        // Schritt 2: PassengerName aufsplitten
        var names = request.PassengerName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string firstName = names.ElementAtOrDefault(0) ?? "";
        string lastName = names.ElementAtOrDefault(1) ?? "";

        // Schritt 3: neue Buchung erstellen
        var booking = new Booking
        {
            FlightId = request.FlightId,
            PassengerId = 0, // Optional: später durch echte User-Logik ersetzen
            PassengerFirstname = firstName,
            PassengerLastname = lastName,
            TicketCount = request.SeatsBooked,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _bookingService.AddAsync(booking);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }
}
