﻿using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTrade.Ordering.Entities;

public static class ServiceCollectionExtensions
{
    public static void AddEntities(this IServiceCollection services, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var dbConfig = configuration.GetConnectionString("DefaultConnection")!;

        services.AddLinqToDBContext<OrderingDataConnection>((provider, options)
            => options
                .UsePostgreSQL(dbConfig)
                .UseDefaultLogging(provider));
    }
}