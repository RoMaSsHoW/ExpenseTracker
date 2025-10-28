using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

namespace ExpenseTracker.Domain.TransactionAggregate;

public class Category : Entity
{
    private readonly List<Transaction>  _transactions;
    
    protected Category()
    {
        
    }
    
    public string Name { get; private set; }
    public TransactionType Type { get; private set; }
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
}