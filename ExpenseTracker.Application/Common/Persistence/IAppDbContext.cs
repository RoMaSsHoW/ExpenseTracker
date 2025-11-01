using ExpenseTracker.Domain.ProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseTracker.Application.Common.Persistence
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Account> Accounts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task MigrateAsync();
    }
}
