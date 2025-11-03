using ExpenseTracker.Application.Common.Persistence;

namespace ExpenseTracker.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAppDbContext _dbContext;
    
    public UnitOfWork(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}