namespace ExpenseTracker.Application.Models.AccountDTOs;

public class AccountGetDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Balance  { get; init; }
    public string CurrencyName { get; init; }
    public bool IsDefault { get; init; }
}