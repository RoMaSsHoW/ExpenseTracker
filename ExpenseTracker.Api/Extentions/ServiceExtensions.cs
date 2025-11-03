using ExpenseTracker.Api.Middlewares;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using ExpenseTracker.Api.Common.Services;
using ExpenseTracker.Application;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;

namespace ExpenseTracker.Api.Extentions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);

        ConfigureJwtAuthenticationAndAuthorization(services, configuration);

        services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));

        services.AddHttpContextAccessor();

        services.AddScoped<IHttpAccessor, HttpAccessor>();
        
        services.AddApplicationServices();
        
        services.AddTransient<AuthorizationFixMiddleware>();

        return services;
    }

    private static void ConfigureJwtAuthenticationAndAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT settings are not configured properly.");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = signingKey
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(new
                        {
                            errors = new
                            {
                                message = "Unauthorized: токен отсутствует, просрочен или недействителен."
                            }
                        });

                        await context.Response.WriteAsync(json);
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(new
                        {
                            errors = new
                            {
                                message = "Forbidden: у вас нет доступа к этому ресурсу."
                            }
                        });

                        await context.Response.WriteAsync(json);
                    },

                    OnAuthenticationFailed = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(new
                        {
                            errors = new
                            {
                                message = "Authentication failed: " + context.Exception.Message
                            }
                        });

                        await context.Response.WriteAsync(json);
                    }
                };
            });

        services.AddAuthorization();
    }
}