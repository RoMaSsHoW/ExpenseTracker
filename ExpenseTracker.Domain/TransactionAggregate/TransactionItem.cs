using ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

namespace ExpenseTracker.Domain.TransactionAggregate;

public class TransactionItem
{
    public TransactionSource TransactionSource { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public decimal Amount { get; set; }
    public Guid TransactionId { get; set; }

    
}