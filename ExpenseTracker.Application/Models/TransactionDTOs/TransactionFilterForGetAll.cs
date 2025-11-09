namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionFilterForGetAll
{
    public string? Name { get; init; }
    public decimal? AmountFrom { get; init; }
    public decimal? AmountTo { get; init; }
    public Guid? CategoryId { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}