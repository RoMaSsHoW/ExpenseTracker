using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Application.Models.AccountDTOs;

public class AccountCreateDTO
{
    public string Name { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public int CurrencyId { get; init; } = Currency.UZS.Id;
    public bool IsDefault { get; init; } = false;
}