using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Application.Models.AccountDTOs;

public class AccountGetDTO
{
    public AccountGetDTO(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Balance = account.Balance;
        Currency = account.Currency;
        UserId = account.UserId;
        IsDefault = account.IsDefault;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Balance  { get; init; }
    public Currency Currency { get; init; }
    public Guid UserId { get; init; }
    public bool IsDefault { get; init; }
}