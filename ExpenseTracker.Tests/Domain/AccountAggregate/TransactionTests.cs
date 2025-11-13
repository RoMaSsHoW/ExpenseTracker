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
    
    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("   ")]
    // public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    // {
    //     // Act & Assert
    //     var act = () => Transaction.Create(
    //         name: invalidName!,
    //         amount: 100m,
    //         currencyId: Currency.USD.Id,
    //         transactionTypeId: TransactionType.Income.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         accountId: _accountId,
    //         date: null,
    //         description: null,
    //         categoryId: null);
    //
    //     act.Should().Throw<ArgumentException>()
    //         .WithMessage("*Transaction name cannot be null or empty.*");
    // }
    //
    // [Theory]
    // [InlineData(0)]
    // [InlineData(-50)]
    // public void Create_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal invalidAmount)
    // {
    //     // Act & Assert
    //     var act = () => Transaction.Create(
    //         name: "Тест",
    //         amount: invalidAmount,
    //         currencyId: Currency.USD.Id,
    //         transactionTypeId: TransactionType.Income.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         accountId: _accountId,
    //         date: null,
    //         description: null,
    //         categoryId: null);
    //
    //     act.Should().Throw<ArgumentOutOfRangeException>()
    //         .WithMessage("*Transaction amount must be positive.*");
    // }
    //
    // [Fact]
    // public void Create_WithEmptyAccountId_ShouldThrowArgumentException()
    // {
    //     // Act & Assert
    //     var act = () => Transaction.Create(
    //         name: "Тест",
    //         amount: 100m,
    //         currencyId: Currency.USD.Id,
    //         transactionTypeId: TransactionType.Income.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         accountId: Guid.Empty,
    //         date: null,
    //         description: null,
    //         categoryId: null);
    //
    //     act.Should().Throw<ArgumentException>()
    //         .WithMessage("*AccountId must be a valid GUID.*");
    // }
    //
    // [Fact]
    // public void Rename_WithValidName_ShouldUpdateName()
    // {
    //     // Arrange
    //     var transaction = CreateSampleTransaction();
    //
    //     // Act
    //     transaction.Rename("Новая покупка");
    //
    //     // Assert
    //     transaction.Name.Should().Be("Новая покупка");
    // }
    //
    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("   ")]
    // public void Rename_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    // {
    //     // Arrange
    //     var transaction = CreateSampleTransaction();
    //
    //     // Act & Assert
    //     var act = () => transaction.Rename(invalidName!);
    //
    //     act.Should().Throw<ArgumentException>()
    //         .WithMessage("*New name cannot be null or empty.*");
    // }
    //
    // [Fact]
    // public void ChangeCategory_ShouldUpdateCategoryId()
    // {
    //     // Arrange
    //     var transaction = CreateSampleTransaction();
    //     var newCategoryId = Guid.NewGuid();
    //
    //     // Act
    //     transaction.ChangeCategory(newCategoryId);
    //
    //     // Assert
    //     transaction.CategoryId.Should().Be(newCategoryId);
    // }
    //
    // [Fact]
    // public void ChangeCategory_ToNull_ShouldSetCategoryIdToNull()
    // {
    //     // Arrange
    //     var categoryId = Guid.NewGuid();
    //     var transaction = Transaction.Create(
    //         name: "Тест",
    //         amount: 100m,
    //         currencyId: Currency.USD.Id,
    //         transactionTypeId: TransactionType.Expense.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         accountId: _accountId,
    //         date: null,
    //         description: null,
    //         categoryId: categoryId);
    //
    //     // Act
    //     transaction.ChangeCategory(null);
    //
    //     // Assert
    //     transaction.CategoryId.Should().BeNull();
    // }
    //
    // [Theory]
    // [InlineData("  Описание  ", "Описание")]
    // [InlineData("Полное описание", "Полное описание")]
    // [InlineData(null, null)]
    // [InlineData("", null)]
    // [InlineData("   ", null)]
    // public void UpdateDescription_ShouldTrimAndSetDescription(string input, string expected)
    // {
    //     // Arrange
    //     var transaction = CreateSampleTransaction();
    //
    //     // Act
    //     transaction.UpdateDescription(input);
    //
    //     // Assert
    //     transaction.Description.Should().Be(expected);
    // }
    //
    // [Fact]
    // public void Create_ViaAccountAddTransaction_ShouldCreateAndUpdateBalance()
    // {
    //     // Arrange
    //     var userId = Guid.NewGuid();
    //     var account = Account.Create(
    //         existingAccounts: new List<Account>(),
    //         name: "Кошелёк",
    //         initialBalance: 1000m,
    //         currencyId: Currency.USD.Id,
    //         userId: userId,
    //         requestedAsDefault: true);
    //
    //     // Act
    //     var transaction = account.AddTransaction(
    //         name: "Кофе",
    //         amount: 5.5m,
    //         transactionTypeId: TransactionType.Expense.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         date: _fixedDate,
    //         description: "Утренний кофе",
    //         categoryId: null);
    //
    //     // Assert
    //     transaction.Should().NotBeNull();
    //     transaction.Name.Should().Be("Кофе");
    //     transaction.Amount.Should().Be(5.5m);
    //     transaction.Type.Should().Be(TransactionType.Expense);
    //     transaction.Date.Should().Be(_fixedDate);
    //     account.Balance.Should().Be(1000m - 5.5m);
    //     account.Transactions.Should().Contain(transaction);
    // }
    //
    // // Вспомогательный метод
    // private Transaction CreateSampleTransaction()
    // {
    //     return Transaction.Create(
    //         name: "Начальная транзакция",
    //         amount: 100m,
    //         currencyId: Currency.USD.Id,
    //         transactionTypeId: TransactionType.Income.Id,
    //         transactionSourceId: TransactionSource.Manual.Id,
    //         accountId: _accountId,
    //         date: _fixedDate,
    //         description: "Тест",
    //         categoryId: null);
    // }
}