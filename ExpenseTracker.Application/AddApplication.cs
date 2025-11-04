using System.Data;
using ExpenseTracker.Application.Commands.AuthCommands;
using ExpenseTracker.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ExpenseTracker.Application;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        
        ConfigureDapper(services, configuration);
        
        ConfigureMediatR(services);
        
        return services;
    }

    private static void ConfigureDapper(IServiceCollection services, IConfiguration configuration) 
    {
        services.AddScoped<IDbConnection>(provider =>
        {
            return new NpgsqlConnection(configuration.GetConnectionString("PostgresqlDbConnection"));
        });
    }
    
    private static void ConfigureMediatR(IServiceCollection services)
    {
        services.AddMediatR(mc =>
        {
            mc.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly);
        });
    }
}
