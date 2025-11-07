namespace ExpenseTracker.Domain.AccountAggregate.Interfaces;

public interface IAccountRepository
{
    Task<Account> FindByIdAsync(Guid accountId);
    Task<IEnumerable<Account>> FindAllByUserIdAsync(Guid userId);
    Task AddAsync(Account account);
}