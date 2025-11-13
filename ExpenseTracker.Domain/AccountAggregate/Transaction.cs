using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class Transaction : Entity
{
    public Transaction() { }

    private Transaction(
        string name,
        decimal amount,
        Currency currency,
        TransactionType type,
        TransactionSource source,
        Guid accountId,
        DateTime date,
        string? description,
        Guid? categoryId)
    {
        Name = name;
        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Source = source ?? throw new ArgumentNullException(nameof(source));
        AccountId = accountId;
        Date = date;
        Description = description;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionSource Source { get; private set; }
    public Guid AccountId { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Account Account { get; private set; }
    
    internal static Transaction Create(
        string name,
        decimal amount,
        int currencyId,
        int transactionTypeId,
        int transactionSourceId,
        Guid accountId,
        DateTime? date,
        string? description,
        Guid? categoryId)
    {
        Validate(name, amount, accountId); 
        
        var currency = Enumeration.FromId<Currency>(currencyId);
        var transactionType = Enumeration.FromId<TransactionType>(transactionTypeId);
        var transactionSource = Enumeration.FromId<TransactionSource>(transactionSourceId);
        var transactionDate = (date == null || date == default)
            ? DateTime.UtcNow
            : date.Value;
        
        return new Transaction(
            name, 
            amount,
            currency,
            transactionType,
            transactionSource,
            accountId,
            transactionDate,
            description,
            categoryId);
    }
    
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New name cannot be null or empty.", nameof(newName));
        
        Name = newName;
    }
    
    public void ChangeCategory(Guid? newCategoryId)
    {
        CategoryId = newCategoryId;
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description) || string.IsNullOrEmpty(description))
            Description = null;
        else
            Description = description?.Trim();
    }
    
    private static void Validate(string name, decimal amount, Guid accountId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Transaction name cannot be null or empty.", nameof(name));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Transaction amount must be positive.");

        if (accountId == Guid.Empty)
            throw new ArgumentException("AccountId must be a valid GUID.", nameof(accountId));
    }
}