using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Tests.Domain.AccountAggregate;

public class TransactionTests
{
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly DateTime _fixedDate = new DateTime(2025, 4, 1, 12, 0, 0, DateTimeKind.Utc);
    
    [Fact]
    public void Create_WithValidParameters_ShouldCreateTransaction()
    {
        // Act
        var tx = Transaction.Create(
            "Зарплата",
            5000m,
            Currency.USD.Id,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            _accountId,
            _fixedDate,
            "Апрельская зарплата",
            Guid.NewGuid());
        
        // Assert
        Assert.NotNull(tx);
        Assert.Equal("Зарплата", tx.Name);
        Assert.Equal(5000m, tx.Amount);
        Assert.Equal(Currency.USD, tx.Currency);
        Assert.Equal(TransactionType.Income, tx.Type);
        Assert.Equal(TransactionSource.Manual, tx.Source);
        Assert.Equal(_accountId, tx.AccountId);
    }
    
    [Fact]
    public void Create_WithEmptyAccountId_ShouldThrowArgumentException()
    {
        // Act & Assert & Assert
        Assert.Throws<ArgumentException>(() => Transaction.Create(
            "Тест",
            100m,
            Currency.USD.Id,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            Guid.Empty,
            null,
            null,
            null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Act & Assert & Assert
        Assert.Throws<ArgumentException>(() => Transaction.Create(
            invalidName,
            100m,
            Currency.USD.Id,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            Guid.Empty,
            null,
            null,
            null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Create_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal invalidAmount)
    {
        // Act & Assert & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Transaction.Create(
            "Тест",
            invalidAmount,
            Currency.USD.Id,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            Guid.Empty,
            null,
            null,
            null));
    }
    
    [Fact]
    public void Rename_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var transaction = CreateSampleTransaction();
    
        // Act
        transaction.Rename("Новая покупка");
    
        // Assert
        Assert.Equal("Новая покупка",  transaction.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var transaction = CreateSampleTransaction();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => transaction.Rename(invalidName!));
    }
    
    [Fact]
    public void ChangeCategory_ShouldUpdateCategoryId()
    {
        // Arrange
        var transaction = CreateSampleTransaction();
        var newCategoryId = Guid.NewGuid();
    
        // Act
        transaction.ChangeCategory(newCategoryId);
    
        // Assert
        Assert.Equal(newCategoryId, transaction.CategoryId);
    }
    
    [Fact]
    public void ChangeCategory_ToNull_ShouldSetCategoryIdToNull()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var transaction = Transaction.Create(
            "Тест",
            100m,
            Currency.USD.Id,
            TransactionType.Expense.Id,
            TransactionSource.Manual.Id,
            _accountId,
            null,
            null,
            categoryId);
    
        // Act
        transaction.ChangeCategory(null);
    
        // Assert
        Assert.Equal(null, transaction.CategoryId);
    }
    
    [Theory]
    [InlineData("  Описание  ", "Описание")]
    [InlineData("Полное описание", "Полное описание")]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    public void UpdateDescription_ShouldTrimAndSetDescription(string input, string expected)
    {
        // Arrange
        var transaction = CreateSampleTransaction();
    
        // Act
        transaction.UpdateDescription(input);
    
        // Assert
        Assert.Equal(expected, transaction.Description);
    }
    
    private Transaction CreateSampleTransaction()
    {
        return Transaction.Create(
            "Начальная транзакция",
            100m,
            Currency.USD.Id,
            TransactionType.Income.Id,
            TransactionSource.Manual.Id,
            _accountId,
            _fixedDate,
            "Тест",
            null);
    }
}