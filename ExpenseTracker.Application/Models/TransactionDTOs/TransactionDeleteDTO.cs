namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionDeleteDTO
{
    public List<Guid> Ids { get; init; }
}