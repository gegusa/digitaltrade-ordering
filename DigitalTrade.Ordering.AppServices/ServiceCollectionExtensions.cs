using DigitalTrade.Ordering.AppServices.Ordering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTrade.Ordering.AppServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderingHandler, OrderingHandler>();

        return services;
    }
}