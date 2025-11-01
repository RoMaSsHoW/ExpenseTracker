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
    
    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentNullException(nameof(newName), "Account name cannot be null or empty.");

        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName.Trim().ToLower());
    }

    public void ChangeCurrency(int currencyId)
    {
        Currency = Enumeration.FromId<Currency>(currencyId)
                   ?? throw new ArgumentOutOfRangeException(nameof(currencyId), "Invalid currency ID.");
    }

    public void SetAsDefault()
    {
        IsDefault = true;
    }
    
    public void UnsetAsDefault()
    {
        IsDefault = false;
    }
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Deposit amount must be greater than zero.");

        Balance += amount;
    }
    
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Withdrawal amount must be greater than zero.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds for withdrawal.");

        Balance -= amount;
    }
}