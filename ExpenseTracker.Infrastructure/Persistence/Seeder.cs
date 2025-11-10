using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.CategoryAggregate;
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
        await SeedRecurringRule();
        await SeedCategories();
    }

    private async Task SeedUsers()
    {
        if (await _dbContext.Users.AnyAsync()) 
            return;

        var admin = User.Register(
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
        
        var accounts = await _dbContext.Accounts
            .Where(a => a.UserId == admin.Id)
            .ToListAsync();

        var account = Account.Create(
            accounts,
            "Test",
            100_000m,
            Currency.UZS.Id,
            admin.Id,
            true);
        
        account.AddTransaction(
            "Initial balance",
            23_000m,
            TransactionType.Expense.Id,
            TransactionSource.Manual.Id,
            DateTime.UtcNow.AddHours(-4),
            "Seed transaction for testing",
            null);
            
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedRecurringRule()
    {
        if (await _dbContext.RecurringRules.AnyAsync())
            return;
        
        var admin = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Address == "admin@gmail.com" &&
                                      u.Role == Role.Admin);
        
        if (admin is null)
            return;
        
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Name == "Test" && a.UserId == admin.Id);
        
        if (account is null)
            return;
        
        account.AddRecurringRule(
            name: "Daily test expense",
            amount: 5_000m,
            categoryId: null,
            transactionTypeId: TransactionType.Expense.Id,
            recurringFrequencyId: RecurringFrequency.Daily.Id, // предположим, что есть перечисление
            startDate: DateTime.UtcNow // сегодня
        );
        
        await _dbContext.SaveChangesAsync();
    }
    
    private async Task SeedCategories()
    {
        if (await _dbContext.Categories.AnyAsync())
            return;
        
        var admin = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Address == "admin@gmail.com" &&
                                      u.Role == Role.Admin);

        var category = Category.CreateDefault(
            "Test",
            TransactionType.Expense.Id,
            admin.Id);
        
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }
}