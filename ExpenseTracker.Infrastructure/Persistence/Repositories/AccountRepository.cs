using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IAppDbContext _dbContext;
    
    public AccountRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<Account> FindByIdAsync(Guid accountId)
    {
        var account = _dbContext.Accounts
            .Include(a => a.Transactions)
            .Include(a => a.RecurringRules)
            .FirstOrDefaultAsync(a => a.Id == accountId);
        
        if (account is null)
            throw new KeyNotFoundException($"Account with ID '{accountId}' was not found.");
        
        return account;
    }

    public async Task<IEnumerable<Account>> FindAllByUserIdAsync(Guid userId)
    {
        var accounts = await _dbContext.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
        
        return accounts;
    }

    public async Task<Account> FindDefaultByUserIdAsync(Guid userId)
    {
        var account = await _dbContext.Accounts
            .Include(a => a.Transactions)
            .Include(a => a.RecurringRules)
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        
        if (account is null)
            throw new KeyNotFoundException($"Account with ID '{userId}' was not found.");
        
        return account;
    }

    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
    }

    public void Remove(Guid accountId)
    {
        var account = _dbContext.Accounts.Find(accountId);
        if (account is null) 
            throw new KeyNotFoundException($"Account with ID '{accountId}' was not found.");
        
        _dbContext.Accounts.Remove(account);
    }
}