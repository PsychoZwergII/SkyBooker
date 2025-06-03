using FlightService.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FlightService.Services;

public class FlightService : IFlightService
{
    private readonly IMongoCollection<Flight> _flights;

    public FlightService(IConfiguration config)
    {
        var client = new MongoClient(config["FlightDatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["FlightDatabaseSettings:DatabaseName"]);
        _flights = database.GetCollection<Flight>(config["FlightDatabaseSettings:CollectionName"]);
    }

    public async Task<List<Flight>> GetAllAsync() => await _flights.Find(_ => true).ToListAsync();

    public async Task<Flight?> GetByIdAsync(string id) =>
        await _flights.Find(f => f.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(Flight flight)
    {
        // 🟢 ID vorher erzeugen
        flight.Id = ObjectId.GenerateNewId().ToString(); // <-- Wichtig!
        await _flights.InsertOneAsync(flight);
    }
}
