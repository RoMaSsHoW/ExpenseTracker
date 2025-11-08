namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionEditDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid? CategoryId { get; init; }
    public string? Description { get; init; }
}