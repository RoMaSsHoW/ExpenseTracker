using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class RecurringRule : Entity
{
    public RecurringRule() { }
    
    private RecurringRule(
        string name,
        decimal amount,
        int currencyId,
        Guid? categoryId,
        int transactionTypeId,
        int recurringFrequencyId,
        DateTime startDate,
        Guid accountId)
    {
        Name = name;
        Amount = amount;
        Currency = Enumeration.FromId<Currency>(currencyId);
        CategoryId = categoryId;
        Type = Enumeration.FromId<TransactionType>(transactionTypeId);
        Frequency = Enumeration.FromId<RecurringFrequency>(recurringFrequencyId);
        NextRunAt = startDate.Date <= DateTime.UtcNow.Date ? CalculateNextRun(startDate) : startDate;
        AccountId = accountId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public RecurringFrequency Frequency { get; private set; }
    public DateTime NextRunAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; }

    public static RecurringRule Create(
        string name,
        decimal amount,
        int currencyId,
        Guid? categoryId,
        int transactionTypeId,
        int recurringFrequencyId,
        DateTime startDate,
        Guid accountId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Recurring rule name cannot be null or empty.");
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Recurring rule amount cannot be zero or negative.");
        
        return new RecurringRule(
            name, 
            amount,
            currencyId,
            categoryId,
            transactionTypeId,
            recurringFrequencyId,
            startDate,
            accountId);
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
    
    public void CreateAutoTransaction()
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot create auto transaction for inactive recurring rule.");
        
        Account.AddTransaction(
            Name, 
            Amount,
            Type.Id,
            TransactionSource.Auto.Id, 
            NextRunAt,
            null,
            CategoryId);

        NextRunAt = CalculateNextRun(NextRunAt);
    }
    
    private DateTime CalculateNextRun(DateTime fromDate) =>
        Frequency switch
        {
            var t when Equals(t, RecurringFrequency.Daily) => fromDate.AddDays(1),
            var t when Equals(t, RecurringFrequency.Weekly) => fromDate.AddDays(7),
            var t when Equals(t, RecurringFrequency.Monthly) => fromDate.AddMonths(1),
            _ => throw new ArgumentException("Unsupported recurring frequency")
        };
}
