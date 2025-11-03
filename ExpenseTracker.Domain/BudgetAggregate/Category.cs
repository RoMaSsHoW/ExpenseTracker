using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.BudgetAggregate;

public class Category : Entity
{
    public Category() { }
    
    private Category(
        string name,
        int transactionTypeId,
        Guid? userId,
        bool isDefault)
    {
        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLower());
        Type = Enumeration.FromId<TransactionType>(transactionTypeId);
        UserId = userId;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid? UserId { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public bool IsDefault { get; private set; }
    
    public static Category Create(
        string name,
        int transactionTypeId,
        Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        if (userId == Guid.Empty)
            throw new ArgumentException("Category user id cannot be empty.", nameof(userId));
        
        return new Category(name, transactionTypeId, userId, false);
    }

    public static Category CreateDefault(
        string name,
        int transactionTypeId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));
        
        return new Category(name, transactionTypeId, null, true);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New category name cannot be empty.", nameof(newName));

        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName.Trim().ToLower());
    }
}