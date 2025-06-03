using BookService.Models;
using BookService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public BookingController(BookingContext context, IHttpClientFactory factory, IConfiguration config)
    {
        _context = context;
        _httpClientFactory = factory;
        _config = config;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
    {
        return await _context.Bookings.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetById(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Booking booking)
    {
        // 🔁 AO4: Validierung per HTTP-Call an FlightService
        var client = _httpClientFactory.CreateClient();
        var flightServiceUrl = _config["FlightServiceUrl"]; // z. B. http://flightservice:80
        var response = await client.GetAsync($"{flightServiceUrl}/api/flight/{booking.FlightId}");

        if (!response.IsSuccessStatusCode)
            return BadRequest("Ungültige Flugnummer (FlightId nicht gefunden)");

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }
}
