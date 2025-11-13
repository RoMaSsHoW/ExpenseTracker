using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.CategoryAggregate;

public class Category : Entity
{
    public Category() { }
    
    private Category(
        string name,
        TransactionType type,
        Guid userId,
        bool isDefault)
    {
        Name = FormatName(name);
        Type = type ?? throw new ArgumentNullException(nameof(type));
        UserId = userId;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid UserId { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public bool IsDefault { get; private set; }
    
    public static Category Create(
        string name,
        int transactionTypeId,
        Guid userId)
    {
        ValidateCreationParameters(name, userId);
        
        var transactionType = Enumeration.FromId<TransactionType>(transactionTypeId);
        
        return new Category(name, transactionType, userId, false);
    }

    public static Category CreateDefault(
        string name,
        int transactionTypeId,
        Guid userId)
    {
        ValidateCreationParameters(name, userId);
        
        var transactionType = Enumeration.FromId<TransactionType>(transactionTypeId);
        
        return new Category(name, transactionType, userId, true);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New category name cannot be empty.", nameof(newName));

        Name = FormatName(newName);
    }

    public void ChangeType(int newTransactionTypeId)
    {
        Type = Enumeration.FromId<TransactionType>(newTransactionTypeId);   
    }
    
    private static string FormatName(string name) =>
        CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLower());
    
    private static void ValidateCreationParameters(string name, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        if (userId == Guid.Empty)
            throw new ArgumentException("Category user id cannot be empty.", nameof(userId));
    }
}