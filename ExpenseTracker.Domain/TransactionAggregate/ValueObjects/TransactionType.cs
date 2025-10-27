using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

public class TransactionType : Enumeration
{
    public static readonly TransactionType Income = new TransactionType(1, "Income");
    public static readonly TransactionType Outcome = new TransactionType(2, "Outcome");
    
    public TransactionType(int id, string name) 
        : base(id, name)
    { }
}