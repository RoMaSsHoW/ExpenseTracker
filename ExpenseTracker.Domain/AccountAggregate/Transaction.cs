using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class Transaction : Entity
{
    public Transaction() { }

    private Transaction(
        string name,
        decimal amount,
        int currencyId,
        int transactionTypeId,
        int transactionSourceId,
        Guid accountId,
        DateTime date,
        string? description,
        Guid? categoryId)
    {
        Name = name;
        Amount = amount;
        Currency = Enumeration.FromId<Currency>(currencyId);
        Type =  Enumeration.FromId<TransactionType>(transactionTypeId);
        Source = Enumeration.FromId<TransactionSource>(transactionSourceId);
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
        var transactionDate = (date == null || date == default)
            ? DateTime.UtcNow
            : date.Value;
        
        Validate(name, amount, accountId); 
        
        return new Transaction(
            name, 
            amount,
            currencyId,
            transactionTypeId,
            transactionSourceId,
            accountId,
            transactionDate,
            description,
            categoryId);
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
        Description = description?.Trim();
    }
}