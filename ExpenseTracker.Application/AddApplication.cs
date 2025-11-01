using ExpenseTracker.Application.Commands.AuthCommands;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Application;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        
        ConfigureMediatR(services);
        
        return services;
    }

    private static void ConfigureMediatR(IServiceCollection services)
    {
        services.AddMediatR(mc =>
        {
            mc.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly);
        });
    }
}