using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using GatewayService.Services;

var builder = WebApplication.CreateBuilder(args);

// Ocelot config laden
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register GatewayService
builder.Services.AddHttpClient();
builder.Services.AddScoped<IGatewayService, GatewayService.Services.GatewayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.UseOcelot();  // WICHTIG

app.Run();
