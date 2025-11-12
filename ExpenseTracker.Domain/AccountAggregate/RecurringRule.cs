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
        NextRunAt = startDate.Date == DateTime.UtcNow.Date ? startDate.AddDays(1) : startDate;
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
        if (startDate.Date < DateTime.UtcNow.Date)
            throw new ArgumentOutOfRangeException(nameof(startDate), "Start date cannot be in the past.");
        
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

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentNullException(nameof(newName), "Recurring rule name cannot be null or empty.");   
        
        Name = newName;
    }
    
    public void ChangeAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(newAmount), "Recurring rule amount cannot be zero or negative.");
        
        Amount = newAmount;
    }

    public void ChangeCategory(Guid? newCategoryId)
    {
        CategoryId = newCategoryId;
    }
    
    public void ChangeType(int newTransactionTypeId)
    {
        Type = Enumeration.FromId<TransactionType>(newTransactionTypeId);
    }

    public void ChangeFrequency(int newRecurringFrequencyId)
    {
        Frequency = Enumeration.FromId<RecurringFrequency>(newRecurringFrequencyId);
    }

    public void ChangeNextRunAt(DateTime newNextRunAt)
    {
        if (newNextRunAt.Date < DateTime.UtcNow.Date)
            throw new ArgumentOutOfRangeException(nameof(newNextRunAt), "Start date cannot be in the past.");

        NextRunAt = newNextRunAt.Date == DateTime.UtcNow.Date ? CalculateNextRun(newNextRunAt) : newNextRunAt;
    }
    
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
