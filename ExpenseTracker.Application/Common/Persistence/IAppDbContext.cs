using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseTracker.Application.Common.Persistence;

public interface IAppDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Account> Accounts { get; }
    public DbSet<Transaction> Transactions { get; }
    public DbSet<Category> Categories { get; }
    public DbSet<RecurringRule>  RecurringRules { get; }

    ChangeTracker ChangeTracker { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task MigrateAsync();
}