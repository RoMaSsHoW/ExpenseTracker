using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class RecurringFrequency : Enumeration
{
    public static readonly RecurringFrequency Daily = new RecurringFrequency(1, nameof(Daily));
    public static readonly RecurringFrequency Weekly = new RecurringFrequency(2, nameof(Weekly));
    public static readonly RecurringFrequency Monthly = new RecurringFrequency(3, nameof(Monthly));
    
    public RecurringFrequency(int id, string name) 
        : base(id, name)
    { }
}