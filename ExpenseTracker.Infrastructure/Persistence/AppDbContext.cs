using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) 
    { }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RecurringRule> RecurringRules => Set<RecurringRule>();

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}