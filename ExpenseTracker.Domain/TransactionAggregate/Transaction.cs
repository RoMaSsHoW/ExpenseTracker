using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

namespace ExpenseTracker.Domain.TransactionAggregate;

public class Transaction : Entity
{
    private readonly List<TransactionItem> _items;

    protected Transaction()
    {
        
    }
    
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public IReadOnlyCollection<TransactionItem> Items => _items.AsReadOnly();
}