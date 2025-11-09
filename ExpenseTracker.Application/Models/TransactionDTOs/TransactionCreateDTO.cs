namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionCreateDTO
{
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public int TransactionTypeId { get; init; }
    public DateTime? Date { get; init; }
    public Guid? CategoryId { get; init; }
}