namespace ExpenseTracker.Application.Models.AccountDTOs;

public class AccountEditDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
}