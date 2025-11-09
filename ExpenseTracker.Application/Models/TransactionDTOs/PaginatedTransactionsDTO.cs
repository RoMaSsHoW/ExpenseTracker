namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class PaginatedTransactionsDTO
{
    public IEnumerable<TransactionViewDTO> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}