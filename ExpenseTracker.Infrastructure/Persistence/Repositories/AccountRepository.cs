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
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Account>> FindAllByUserIdAsync(Guid userId)
    {
        var accounts = await _dbContext.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
        
        return accounts;
    }

    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
    }
}