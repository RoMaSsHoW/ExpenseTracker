using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class TransactionSource : Enumeration
{
    public static readonly TransactionSource Manual = new TransactionSource(1, nameof(Manual));
    public static readonly TransactionSource Imported = new TransactionSource(2, nameof(Imported));
    public static readonly TransactionSource Auto = new TransactionSource(3, nameof(Auto));
    
    public TransactionSource(int id, string name) 
        : base(id, name)
    { }
}