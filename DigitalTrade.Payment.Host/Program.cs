using KafkaFlow;
using DigitalTrade.Payment.Entities;
using DigitalTrade.Payment.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEntities(configuration);
builder.Services.AddKafkaFlow(configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();

app.Run();