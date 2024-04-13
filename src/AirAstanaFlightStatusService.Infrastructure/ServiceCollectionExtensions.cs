using AirAstanaFlightStatusService.Application.Interfaces.Persistence;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using AirAstanaFlightStatusService.Infrastructure.Persistence;
using AirAstanaFlightStatusService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirAstanaFlightStatusService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructurePersistence(this IServiceCollection services, IConfiguration configuration, string environmentName)
    {
        var connectionString = configuration.GetConnectionString("Ftgo")!;

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        3,
                        TimeSpan.FromSeconds(10),
                        null);
                });
        });

        return services;
    }

    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IDataContext, DataContext>();

        return services;
    }
}