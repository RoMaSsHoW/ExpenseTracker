using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Tests.Domain.AccountAggregate;

public class AccountTests
{
    [Fact]
    public void Create_ShouldInitializeFirstAccount_AsDefault()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingAccounts = new List<Account>();

        // Act
        var account = Account.Create(
            existingAccounts,
            "кошелёк",
            100,
            Currency.USD.Id,
            userId,
            false);

        // Assert
        Assert.NotNull(account);
        Assert.Equal("Кошелёк", account.Name);
        Assert.Equal(100m, account.Balance);
        Assert.Equal(Currency.USD, account.Currency);
        Assert.Equal(userId, account.UserId);
        Assert.True(account.IsDefault);
        Assert.NotEmpty(account.Transactions);
        Assert.Empty(account.RecurringRules);
    }
    
    [Fact]
    public void Create_ShouldUnsetPreviousDefault_WhenNewAccountIsDefault()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var acc1 = Account.Create(
            [],
            "Первый",
            0,
            Currency.USD.Id,
            userId,
            true);
        
        var existingAccounts = new List<Account> { acc1 };

        // Act
        var acc2 = Account.Create(
            existingAccounts,
            "Второй",
            0,
            Currency.USD.Id,
            userId,
            true);

        // Assert
        Assert.False(acc1.IsDefault);
        Assert.True(acc2.IsDefault);
    }

    [Fact]
    public void SetAsDefault_ShouldUnsetOtherDefaults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var acc1 = Account.Create(
            [],
            "Первый",
            0,
            Currency.USD.Id,
            userId,
            true);
        
        var acc2 = Account.Create(
            [],
            "Второй",
            0,
            Currency.USD.Id,
            userId,
            true);
        
        var accounts = new List<Account> { acc1, acc2 };
        
        // Act
        acc2.SetAsDefault(accounts);
        
        // Assert
        Assert.False(acc1.IsDefault);
        Assert.True(acc2.IsDefault);
    }
    
    [Fact]
    public void Rename_ShouldChangeNameWithFormatting()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "старое имя",
            0,
            Currency.USD.Id,
            Guid.NewGuid(),
            false);

        // Act
        acc.Rename("новое имя");

        // Assert
        Assert.Equal("Новое Имя", acc.Name);
    }
    
    [Fact]
    public void Rename_ShouldThrow_WhenNameIsEmpty()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "имя",
            0,
            Currency.USD.Id, 
            Guid.NewGuid(),
            false);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => acc.Rename("  "));
    }
    
    [Fact]
    public void AddTransaction_ShouldIncreaseBalance_ForIncome()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            0,
            Currency.USD.Id,
            Guid.NewGuid(),
            true);

        // Act
        acc.AddTransaction(
            "Зарплата",
            500,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            DateTime.UtcNow,
            null,
            null);

        // Assert
        Assert.Equal(500, acc.Balance);
        Assert.Single(acc.Transactions);
    }
    
    [Fact]
    public void AddTransaction_ShouldDecreaseBalance_ForExpense()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            1000, 
            Currency.USD.Id,
            Guid.NewGuid(),
            true);

        // Act
        acc.AddTransaction(
            "Покупка",
            200,
            TransactionType.Expense.Id,
            TransactionSource.Manual.Id,
            DateTime.UtcNow,
            null, 
            null);
        
        // Assert
        Assert.Equal(800, acc.Balance);
        Assert.Equal(2, acc.Transactions.Count);
    }

    [Fact]
    public void RemoveTransaction_ShouldRestoreBalance_AfterRemovingExpense()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            1000,
            Currency.USD.Id,
            Guid.NewGuid(),
            true);
        
        var tx = acc.AddTransaction(
            "Покупка",
            200,
            TransactionType.Expense.Id,
            TransactionSource.Manual.Id,
            DateTime.UtcNow,
            null,
            null);

        // Act
        acc.RemoveTransaction(tx.Id);

        // Assert
        Assert.Equal(1000, acc.Balance);
        Assert.Single(acc.Transactions);
    }

    [Fact]
    public void RemoveTransaction_ShouldThrow_WhenNotFound()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            0,
            Currency.USD.Id,
            Guid.NewGuid(),
            true);
        
        var id = Guid.NewGuid();
        
        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => acc.RemoveTransaction(id));

        // Assert
        Assert.Equal($"Transaction with ID '{id}' not found.", ex.Message);
    }
    
    [Fact]
    public void AddRecurringRule_ShouldAddRule()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            0,
            Currency.USD.Id,
            Guid.NewGuid(),
            true);
        
        // Act
        var rule = acc.AddRecurringRule(
            "Аренда",
            1500,
            null,
            TransactionType.Expense.Id,
            RecurringFrequency.Monthly.Id,
            DateTime.UtcNow.AddDays(1));
        
        // Assert
        Assert.Single(acc.RecurringRules);
        Assert.Contains(rule, acc.RecurringRules);
        Assert.Equal("Аренда", rule.Name);
        Assert.True(rule.IsActive);
    }

    [Fact]
    public void RemoveRecurringRule_ShouldThrow_WhenRuleNotFound()
    {
        // Arrange
        var acc = Account.Create(
            [],
            "Карта",
            0,
            Currency.USD.Id,
            Guid.NewGuid(),
            true);
        
        var id = Guid.NewGuid();
        
        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => acc.RemoveRecurringRule(id));

        // Assert
        Assert.Equal($"Rule with ID '{id}' not found.", ex.Message);
    }
}