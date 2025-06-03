using FlightService.Models;
using FlightService.Services;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<FlightService.Services.FlightService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
