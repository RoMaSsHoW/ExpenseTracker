namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionCreateFromDocumentDTO
{
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public string TypeName { get; init; }
    public DateTime? Date { get; init; }
    public string CategoryName { get; init; }
}