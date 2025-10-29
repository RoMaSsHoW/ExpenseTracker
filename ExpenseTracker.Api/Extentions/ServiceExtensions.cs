using ExpenseTracker.Infrastructure;

namespace ExpenseTracker.Api.Extentions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);

        return services;
    }
}