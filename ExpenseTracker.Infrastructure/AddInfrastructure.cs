using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.UserAggregate.Interfaces;
using ExpenseTracker.Infrastructure.Jobs;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.Persistence.Repositories;
using ExpenseTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("AutoTransactionJob");

            q.AddJob<AutoTransactionJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("AutoTransactionJob-trigger")
                // Каждый день в 01:00 ночи по UTC
                .WithCronSchedule("0 0 1 * * ?") // секунда=0, минута=0, час=1, каждый день
                .WithDescription("Run AutoTransactionJob daily at 01:00 UTC"));
        });
        
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        
        return services;
    }
}

