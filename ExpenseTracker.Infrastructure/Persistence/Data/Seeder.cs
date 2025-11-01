using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.Common.ValueObjects;
using ExpenseTracker.Domain.ProfileAggregate;
using ExpenseTracker.Domain.ProfileAggregate.ValueObjects;
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
        await SeedAccounts();
    }

    private async Task SeedUsers()
    {
        if (await _dbContext.Users.AnyAsync()) 
            return;

        var admin = User.Registration("admin", "admin", "admin@gmail.com", "admin123", Role.Admin.Id, _tokenService.GenerateRefreshToken());
        
        await _dbContext.Users.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedAccounts()
    {
        if (await _dbContext.Accounts.AnyAsync())
            return;
        
        var admin = await _dbContext.Users
            .Include(u  => u.Accounts)
            .FirstOrDefaultAsync(u => u.Email.Address == "admin@gmail.com" &&
                                      u.Role == Role.Admin);
        if (admin is null)
            return;
        
        admin.AddAccount("Test", 100_000, Currency.UZB.Id, true);
        await _dbContext.SaveChangesAsync();
    }
}