using System.Globalization;
using ExpenseTracker.Domain.Common.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class Account : Entity
{
    public Account() { }

    private Account(
        string name,
        decimal balance,
        int currencyId,
        Guid userId,
        bool isDefault)
    {
        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLower());
        Balance = balance;
        Currency = Enumeration.FromId<Currency>(currencyId);
        UserId = userId;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }
    public decimal Balance  { get; private set; }
    public Currency Currency { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public User User { get; private set; }

    internal static Account Create(
        string name,
        decimal balance,
        int currencyId,
        Guid userId,
        bool isDefault)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");
        if (balance < 0)
            throw new ArgumentOutOfRangeException(nameof(balance), balance, $"Account balance cannot be negative.");
        
        return new Account(
            name, 
            balance,
            currencyId,
            userId,
            isDefault);
    }
}