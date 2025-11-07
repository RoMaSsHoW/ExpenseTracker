using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class Currency : Enumeration
{
    public static readonly Currency UZS = new Currency(1, nameof(UZS));
    public static readonly Currency USD = new Currency(2, nameof(USD));
    
    public Currency(int id, string name) 
        : base(id, name)
    { }
}