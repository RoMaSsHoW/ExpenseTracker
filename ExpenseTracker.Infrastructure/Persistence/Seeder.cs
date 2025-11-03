using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.UserAggregate;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence;

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

        var admin = User.Registration(
            "admin", 
            "admin",
            "admin@gmail.com",
            "admin123",
            Role.Admin.Id,
            _tokenService.GenerateRefreshToken());
        
        await _dbContext.Users.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedAccounts()
    {
        if (await _dbContext.Accounts.AnyAsync())
            return;
        
        var admin = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Address == "admin@gmail.com" &&
                                      u.Role == Role.Admin);
        if (admin is null)
            return;

        var account = Account.Create("Test", 100_000, Currency.UZB.Id, admin.Id, true);
        
        account.AddTransaction(
            "Initial balance",
            23_000,
            Currency.UZB.Id,
            TransactionType.Expense.Id,
            TransactionSource.Manual.Id,
            DateTime.UtcNow.AddHours(-4),
            "Seed transaction for testing",
            null);
        
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }
}