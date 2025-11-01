using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class TransactionType : Enumeration
{
    public static readonly TransactionType Income = new TransactionType(1, nameof(Income));
    public static readonly TransactionType Expense = new TransactionType(2, nameof(Expense));
    
    public TransactionType(int id, string name) 
        : base(id, name)
    { }
}