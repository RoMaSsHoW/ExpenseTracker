using ExpenseTracker.Domain.Common.ValueObjects;
using ExpenseTracker.Domain.ProfileAggregate;

namespace ExpenseTracker.Application.Models.ProfileDTOs;

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