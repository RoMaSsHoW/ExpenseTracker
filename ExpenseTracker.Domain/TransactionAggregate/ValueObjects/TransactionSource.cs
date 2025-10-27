using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

public class TransactionSource : Enumeration
{
    public static readonly TransactionSource Manual = new TransactionSource(1, "Manual");
    public static readonly TransactionSource Imported = new TransactionSource(2, "Imported");
    public static readonly TransactionSource Auto = new TransactionSource(3, "Auto");
    
    public TransactionSource(int id, string name) 
        : base(id, name)
    { }
}