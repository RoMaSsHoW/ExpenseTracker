using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class Account : Entity
{
    private readonly List<Transaction> _transactions = [];
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
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public static Account Create(
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
    
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentNullException(nameof(newName), "Account name cannot be null or empty.");

        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName.Trim().ToLower());
    }

    public void SetAsDefault()
    {
        IsDefault = true;
    }
    
    public void UnsetAsDefault()
    {
        IsDefault = false;
    }
    
    internal void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Deposit amount must be greater than zero.");

        Balance += amount;
    }
    
    internal void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Withdrawal amount must be greater than zero.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds for withdrawal.");

        Balance -= amount;
    }

    public void AddTransaction(
        string name,
        decimal amount,
        int currencyId,
        int transactionTypeId,
        int transactionSourceId,
        DateTime date,
        string? description,
        Guid? categoryId)
    {
        if (Currency != Enumeration.FromId<Currency>(currencyId))
            throw new InvalidOperationException("Transaction currency must match account currency.");
        
        var transaction = Transaction.Create(
            name, 
            amount, 
            currencyId,
            transactionTypeId,
            transactionSourceId,
            Id,
            date,
            description,
            categoryId);
        
        if (transaction.Type == TransactionType.Expense)
            Withdraw(amount);
        else if (transaction.Type == TransactionType.Income)
            Deposit(amount);
        
        _transactions.Add(transaction);
    }

    public void RemoveTransaction(Guid transactionId)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
        if (transaction == null)
            throw new InvalidOperationException($"Transaction with ID '{transactionId}' not found.");
        
        if (transaction.Type == TransactionType.Expense)
            Deposit(transaction.Amount);
        else if (transaction.Type == TransactionType.Income)
            Withdraw(transaction.Amount);
        
        _transactions.Remove(transaction);
    }
}