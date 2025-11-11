namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class FilterForGetAllTransactionInDocument
{
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}