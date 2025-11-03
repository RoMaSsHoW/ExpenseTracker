using AspNetCore.Swagger.Themes;
using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Api.Middlewares;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter your access token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
    await dbContext.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    await seeder.RunAsync();
}

app.UseSwagger();
app.UseSwaggerUI(Style.Dark);

app.UseHttpsRedirection();

app.UseMiddleware<AuthorizationFixMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();