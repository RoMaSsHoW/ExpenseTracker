using ExpenseTracker.Application;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Events;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;
using ExpenseTracker.Domain.UserAggregate.Interfaces;
using ExpenseTracker.Infrastructure.Jobs;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.Persistence.Repositories;
using ExpenseTracker.Infrastructure.Services;
using MassTransit;
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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<Seeder>();
        
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IDocumentService, DocumentService>();
        
        ConfigureQuartz(services);
        
        ConfigureRabbitMq(services, configuration);
        
        return services;
    }

    private static void ConfigureQuartz(IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("AutoTransactionJob");

            q.AddJob<AutoTransactionJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("AutoTransactionJob-trigger")
                .WithCronSchedule("0 0 1 * * ?") // Каждый день в 01:00 ночи по UTC (секунда=0, минута=0, час=1)
                .WithDescription("Run AutoTransactionJob daily at 01:00 UTC"));
        });
        
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
    }
    
    private static void ConfigureRabbitMq(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        if (rabbitSettings is null || string.IsNullOrEmpty(rabbitSettings.Host))
            throw new InvalidOperationException("RabbitMQ settings are not configured properly.");

        services.AddMassTransit(x =>
        {
            // Register consumers
            x.AddConsumers(typeof(UserRegisteredEventHandler).Assembly);

            // Configure RabbitMQ
            x.UsingRabbitMq((context, cfg) =>
            {
                // var uri = new Uri($"rabbitmq://{rabbitSettings.Host}:{rabbitSettings.Port}{rabbitSettings.VirtualHost}");

                cfg.Host(rabbitSettings.Host, (ushort)rabbitSettings.Port, rabbitSettings.VirtualHost, h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }   
}

