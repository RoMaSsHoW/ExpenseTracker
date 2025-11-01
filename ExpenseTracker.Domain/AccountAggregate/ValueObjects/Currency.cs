using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class Currency : Enumeration
{
    public static readonly Currency UZB = new Currency(1, nameof(UZB));
    public static readonly Currency USD = new Currency(2, nameof(USD));
    
    public Currency(int id, string name) 
        : base(id, name)
    { }
}