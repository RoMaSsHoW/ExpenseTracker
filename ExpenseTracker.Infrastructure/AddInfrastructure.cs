using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.UserAggregate.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Data;
using ExpenseTracker.Infrastructure.Persistence.Repositories;
using ExpenseTracker.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresqlDbConnection"));
        });

        services.AddScoped<Seeder>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
}

