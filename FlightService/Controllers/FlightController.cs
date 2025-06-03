using FlightService.Models;
using FlightService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly IFlightService _service;

    public FlightController(IFlightService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Flight>>> Get() =>
        await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Flight>> GetById(string id)
    {
        var flight = await _service.GetByIdAsync(id);
        return flight is null ? NotFound() : Ok(flight);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flight flight)
    {
        await _service.AddAsync(flight);
        return CreatedAtAction(nameof(GetById), new { id = flight.Id }, flight);
    }
}
