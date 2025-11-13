using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class Account : Entity
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<RecurringRule> _recurringRules = [];
    public Account() { }

    private Account(
        string name,
        Currency currency,
        Guid userId,
        bool isDefault)
    {
        Name = FormatName(name);
        Balance = 0;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
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
    public IReadOnlyCollection<RecurringRule> RecurringRules => _recurringRules.AsReadOnly();

    public static Account Create(
        List<Account> existingAccounts,
        string name,
        decimal initialBalance,
        int currencyId,
        Guid userId,
        bool requestedAsDefault)
    {
        ValidateCreationParameters(name, existingAccounts);
        
        var currency = Enumeration.FromId<Currency>(currencyId);
        var isDefault = !existingAccounts.Any() || requestedAsDefault;

        if (isDefault && existingAccounts.Any(a => a.IsDefault))
            UnsetCurrentDefault(existingAccounts);
        
        var account = new Account(name, currency, userId, isDefault);

        if(initialBalance != 0) 
            account.RecordInitialBalance(initialBalance);

        return account;
    }
    
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentNullException(nameof(newName), "Account name cannot be null or empty.");

        Name = FormatName(newName);
    }

    public void SetAsDefault(IEnumerable<Account> existingAccounts)
    {
        if (existingAccounts is null)
            throw new ArgumentNullException(nameof(existingAccounts));
        
        if (existingAccounts.Any(a => a.IsDefault))
            UnsetCurrentDefault(existingAccounts);
        
        IsDefault = true;   
    }

    public Transaction AddTransaction(
        string name,
        decimal amount,
        int transactionTypeId,
        int transactionSourceId,
        DateTime? date,
        string? description,
        Guid? categoryId)
    {
        var transaction = Transaction.Create(
            name, 
            amount, 
            Currency.Id,
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
        
        return transaction;
    }

    public void RemoveTransaction(Guid transactionId)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId)
            ?? throw new InvalidOperationException($"Transaction with ID '{transactionId}' not found.");
        
        if (transaction.Type == TransactionType.Expense)
            Deposit(transaction.Amount);
        else if (transaction.Type == TransactionType.Income)
            Withdraw(transaction.Amount);
        
        _transactions.Remove(transaction);
    }

    public RecurringRule AddRecurringRule(
        string name,
        decimal amount,
        Guid? categoryId,
        int transactionTypeId,
        int recurringFrequencyId,
        DateTime startDate)
    {
        var recyrringRule = RecurringRule.Create(
            name,
            amount,
            Currency.Id,
            categoryId,
            transactionTypeId,
            recurringFrequencyId,
            startDate,
            Id);
        
        _recurringRules.Add(recyrringRule);
        
        return recyrringRule;
    }

    public void RemoveRecurringRule(Guid ruleId)
    {
        var rule = _recurringRules.FirstOrDefault(r => r.Id == ruleId)
            ?? throw new InvalidOperationException($"Rule with ID '{ruleId}' not found.");
        
        _recurringRules.Remove(rule);
    }
    
    private void UnsetAsDefault() => IsDefault = false;
    
    private static void ValidateCreationParameters(string name, IEnumerable<Account> existingAccounts)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (existingAccounts is null)
            throw new ArgumentNullException(nameof(existingAccounts));
        
        // if (balance < 0)
        // throw new ArgumentOutOfRangeException(nameof(balance), balance, $"Account balance cannot be negative.");
    }
    
    private static string FormatName(string name) =>
        CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLower());
    
    private static void UnsetCurrentDefault(IEnumerable<Account> existingAccounts)
    {
        var currentDefault = existingAccounts.FirstOrDefault(a => a.IsDefault);
        currentDefault!.UnsetAsDefault();
    }
    
    private void RecordInitialBalance(decimal initialBalance)
    {
        var transactionType = initialBalance > 0 
            ? TransactionType.Income.Id 
            : TransactionType.Expense.Id;
        
        AddTransaction(
            "Стартовый баланс",
            Math.Abs(initialBalance),
            transactionType,
            TransactionSource.Manual.Id,
            DateTime.UtcNow,
            null,
            null);
    }
    
    private void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, 
                "Deposit amount must be greater than zero.");

        Balance += amount;
    }
    
    private void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, 
                "Withdrawal amount must be greater than zero.");

        // if (amount > Balance)
        // throw new InvalidOperationException("Insufficient funds for withdrawal.");

        Balance -= amount;
    }
}