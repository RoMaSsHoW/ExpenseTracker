using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseTracker.Application.Common.Persistence
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Account> Accounts { get; }
        public DbSet<Transaction> Transactions { get; }
        public DbSet<Category> Categories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task MigrateAsync();
    }
}
