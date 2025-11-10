using ExpenseTracker.Api.Middlewares;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using ExpenseTracker.Api.Common.Services;
using ExpenseTracker.Application;
using ExpenseTracker.Application.Common.Services;

namespace ExpenseTracker.Api.Extentions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        SetCorsPolicy(services, configuration);
        
        services.AddInfrastructureServices(configuration);

        ConfigureJwtAuthenticationAndAuthorization(services, configuration);
        services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));

        services.AddHttpContextAccessor();
        services.AddScoped<IHttpAccessor, HttpAccessor>();
        
        services.AddApplicationServices(configuration);
        
        services.AddTransient<AuthorizationFixMiddleware>();

        return services;
    }

    private static void SetCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
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

                        var response = Response<object>.Fail(
                            "Unauthorized: токен отсутствует, просрочен или недействителен.",
                            StatusCodes.Status401Unauthorized
                        );

                        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        await context.Response.WriteAsync(json);
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var response = Response<object>.Fail(
                            "Forbidden: у вас нет доступа к этому ресурсу.",
                            StatusCodes.Status403Forbidden
                        );

                        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        await context.Response.WriteAsync(json);
                    },

                    OnAuthenticationFailed = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = Response<object>.Fail(
                            $"Authentication failed: {context.Exception.Message}",
                            StatusCodes.Status401Unauthorized
                        );

                        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        await context.Response.WriteAsync(json);
                    }
                };
            });

        services.AddAuthorization();
    }
}