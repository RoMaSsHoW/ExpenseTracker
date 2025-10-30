using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Data;

public class Seeder
{
    private readonly IAppDbContext  _dbContext;
    private readonly ITokenService _tokenService;

    public Seeder(IAppDbContext dbContext, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }
    
    public async Task RunAsync()
    {
        await SeedUsers();
    }

    private async Task SeedUsers()
    {
        if (await _dbContext.Users.AnyAsync()) 
            return;

        var admin = User.Registration("admin", "admin", "admin@gmail.com", "admin123", Role.Admin.Id, _tokenService.GenerateRefreshToken());
        
        await _dbContext.Users.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }
}