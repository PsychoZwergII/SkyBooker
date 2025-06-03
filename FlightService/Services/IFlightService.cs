using FlightService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightService.Services
{
    public interface IFlightService
    {
        Task<Flight> CreateFlight(Flight flight);
        Task<IEnumerable<Flight>> GetAllFlights();
        Task<Flight> GetFlightById(string id);
    }
} 