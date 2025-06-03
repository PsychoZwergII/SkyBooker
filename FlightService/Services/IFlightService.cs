using FlightService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightService.Services
{
    public interface IFlightService
    {
        Task<List<Flight>> GetAllAsync();
        Task<Flight?> GetByIdAsync(string id);
        Task AddAsync(Flight flight);
    }
} 