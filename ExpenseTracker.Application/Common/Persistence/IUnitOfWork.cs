namespace ExpenseTracker.Application.Common.Persistence;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}