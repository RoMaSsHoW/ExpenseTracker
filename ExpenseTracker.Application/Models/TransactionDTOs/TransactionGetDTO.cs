namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionGetDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public string CurrencyName { get; init; }
    public string? Description { get; init; }
    public Guid? CategoryId { get; init; }
    public string TypeName { get; init; }
    public Guid AccountId { get; init; }
    public DateTime Date { get; init; }
}