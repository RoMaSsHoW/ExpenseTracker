using ExpenseTracker.Domain.Common.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

namespace ExpenseTracker.Domain.TransactionAggregate;

public class RecurringRule : Entity
{
    protected RecurringRule(
        string title,
        decimal amount,
        int currencyId,
        Guid categoryId,
        int transactionTypeId,
        int recurringFrequencyId,
        DateTime startDate,
        Guid accountId)
    {
        Title = title;
        Amount = amount;
        Currency = Enumeration.FromId<Currency>(currencyId);
        CategoryId = categoryId;
        Type = Enumeration.FromId<TransactionType>(transactionTypeId);
        Frequency = Enumeration.FromId<RecurringFrequency>(recurringFrequencyId);
        NextRunAt = startDate;
        AccountId = accountId;
        IsActive = true;
    }
    
    public string Title { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public RecurringFrequency Frequency { get; private set; }
    public DateTime NextRunAt { get; private set; }
    public bool IsActive { get; private set; }
    public Guid AccountId { get; private set; }

    public static RecurringRule Create(
        string title,
        decimal amount,
        int currencyId,
        Guid categoryId,
        int transactionTypeId,
        int recurringFrequencyId,
        DateTime startDate,
        Guid accountId)
    {
        
        return new RecurringRule(
            title, 
            amount,
            currencyId,
            categoryId,
            transactionTypeId,
            recurringFrequencyId,
            startDate,
            accountId);
    }

    public void Deactivate()
    {
        if (IsActive)
            IsActive = false;
    }

    public void Activate()
    {
        if (!IsActive)
            IsActive = true;
    }

    public void UpdateNextRun()
    {
        NextRunAt = Frequency switch
        {
            var t when Equals(t, RecurringFrequency.Daily) => NextRunAt.AddDays(1),
            var t when Equals(t, RecurringFrequency.Weekly) => NextRunAt.AddDays(7),
            var t when Equals(t, RecurringFrequency.Monthly) => NextRunAt.AddMonths(1),
            _ => throw new ArgumentException("Unsupported recurring frequency")
        };
    }
}
