using DigitalTrade.Ordering.AppServices;
using DigitalTrade.Ordering.Entities;
using DigitalTrade.Ordering.Host.Extensions;
using KafkaFlow;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddMvc();
builder.Services.AddControllers();

builder.Services.AddEntities(configuration);
builder.Services.AddAppServices(configuration);
builder.Services.AddKafkaFlow(configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.Map("/", () => "Hello from Ordering!");
app.MapControllers();
var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();

app.Run();