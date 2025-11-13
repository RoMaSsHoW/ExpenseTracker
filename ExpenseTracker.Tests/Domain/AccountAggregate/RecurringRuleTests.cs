using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Tests.Domain.AccountAggregate;

public class RecurringRuleTests
{
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly DateTime _today = DateTime.UtcNow.Date;
    private readonly DateTime _tomorrow = DateTime.UtcNow.Date.AddDays(1);
    private readonly DateTime _yesterday = DateTime.UtcNow.Date.AddDays(-1);
    
    [Fact]
    public void Create_WithValidParameters_ShouldCreateRecurringRule()
    {
        // Act
        var rule = RecurringRule.Create(
            "Аренда",
            1500m,
            Currency.USD.Id,
            Guid.NewGuid(),
            TransactionType.Expense.Id,
            RecurringFrequency.Monthly.Id,
            _tomorrow,
            _accountId);
   
        // Assert
        Assert.NotNull(rule);
        Assert.Equal("Аренда", rule.Name);
        Assert.Equal(1500m, rule.Amount);
        Assert.Equal(Currency.USD, rule.Currency);
        Assert.Equal(TransactionType.Expense, rule.Type);
        Assert.Equal(RecurringFrequency.Monthly, rule.Frequency);
        Assert.Equal(_accountId, rule.AccountId);
        Assert.True(rule.IsActive);
        Assert.Equal(_tomorrow, rule.NextRunAt); // startDate == tomorrow → NextRunAt = tomorrow
        Assert.True(rule.CreatedAt >= DateTime.UtcNow.AddSeconds(-1));
    }
    
    [Fact]
    public void Create_WithStartDateToday_ShouldSetNextRunAtToTomorrow()
    {
        // Act
        var rule = RecurringRule.Create(
            "Зарплата",
            5000m,
            Currency.USD.Id,
            null,
            TransactionType.Income.Id,
            RecurringFrequency.Monthly.Id,
            _today,
            _accountId);
   
        // Assert
        Assert.Equal(_today.AddDays(1), rule.NextRunAt);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowArgumentNullException(string invalidName)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => RecurringRule.Create(
            invalidName!,
            100m,
            Currency.USD.Id,
            null,
            TransactionType.Income.Id,
            RecurringFrequency.Daily.Id,
            _tomorrow,
            _accountId));
   
        Assert.Contains("Recurring rule name cannot be null or empty.", ex.Message);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Create_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal invalidAmount)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => RecurringRule.Create(
            "Тест",
            invalidAmount,
            Currency.USD.Id,
            null,
            TransactionType.Income.Id,
            RecurringFrequency.Daily.Id,
            _tomorrow,
            _accountId));
   
        Assert.Contains("Recurring rule amount cannot be zero or negative.", ex.Message);
    }
    
    [Fact]
    public void Create_WithStartDateInPast_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => RecurringRule.Create(
            "Тест",
            100m,
            Currency.USD.Id,
            null,
            TransactionType.Income.Id,
            RecurringFrequency.Daily.Id,
            _yesterday,
            _accountId));
   
        Assert.Contains("Start date cannot be in the past.", ex.Message);
    }
    
    
    [Fact]
    public void Rename_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var rule = CreateSampleRule();
   
        // Act
        rule.Rename("Новая подписка");
   
        // Assert
        Assert.Equal("Новая подписка", rule.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_WithInvalidName_ShouldThrowArgumentNullException(string invalidName)
    {
        // Arrange
        var rule = CreateSampleRule();
   
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => rule.Rename(invalidName!));
        Assert.Contains("Recurring rule name cannot be null or empty.", ex.Message);
    }
    
           [Fact]
       public void ChangeAmount_WithPositiveAmount_ShouldUpdateAmount()
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act
           rule.ChangeAmount(2500m);
   
           // Assert
           Assert.Equal(2500m, rule.Amount);
       }
   
       [Theory]
       [InlineData(0)]
       [InlineData(-50)]
       public void ChangeAmount_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal invalidAmount)
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act & Assert
           var ex = Assert.Throws<ArgumentOutOfRangeException>(() => rule.ChangeAmount(invalidAmount));
           Assert.Contains("Recurring rule amount cannot be zero or negative.", ex.Message);
       }
   
       [Fact]
       public void ChangeCategory_ShouldUpdateCategoryId()
       {
           // Arrange
           var rule = CreateSampleRule();
           var newCategoryId = Guid.NewGuid();
   
           // Act
           rule.ChangeCategory(newCategoryId);
   
           // Assert
           Assert.Equal(newCategoryId, rule.CategoryId);
       }
   
       [Fact]
       public void ChangeCategory_ToNull_ShouldSetCategoryIdToNull()
       {
           // Arrange
           var categoryId = Guid.NewGuid();
           var rule = RecurringRule.Create(
               "Тест",
               100m,
               Currency.USD.Id,
               categoryId,
               TransactionType.Expense.Id,
               RecurringFrequency.Monthly.Id,
               _tomorrow,
               _accountId);
   
           // Act
           rule.ChangeCategory(null);
   
           // Assert
           Assert.Null(rule.CategoryId);
       }
   
       [Fact]
       public void ChangeType_WithValidId_ShouldUpdateType()
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act
           rule.ChangeType(TransactionType.Expense.Id);
   
           // Assert
           Assert.Equal(TransactionType.Expense, rule.Type);
       }
   
       [Fact]
       public void ChangeFrequency_WithValidId_ShouldUpdateFrequency()
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act
           rule.ChangeFrequency(RecurringFrequency.Weekly.Id);
   
           // Assert
           Assert.Equal(RecurringFrequency.Weekly, rule.Frequency);
       }
   
       [Fact]
       public void ChangeNextRunAt_WithFutureDate_ShouldUpdateNextRunAt()
       {
           // Arrange
           var rule = CreateSampleRule();
           var newDate = _tomorrow.AddDays(5);
   
           // Act
           rule.ChangeNextRunAt(newDate);
   
           // Assert
           Assert.Equal(newDate, rule.NextRunAt);
       }
   
       [Fact]
       public void ChangeNextRunAt_WithPastDate_ShouldThrowArgumentOutOfRangeException()
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act & Assert
           var ex = Assert.Throws<ArgumentOutOfRangeException>(() => rule.ChangeNextRunAt(_yesterday));
           Assert.Contains("Start date cannot be in the past.", ex.Message);
       }
   
       [Fact]
       public void Activate_ShouldSetIsActiveToTrue()
       {
           // Arrange
           var rule = CreateSampleRule();
           rule.Deactivate();
   
           // Act
           rule.Activate();
   
           // Assert
           Assert.True(rule.IsActive);
       }
   
       [Fact]
       public void Deactivate_ShouldSetIsActiveToFalse()
       {
           // Arrange
           var rule = CreateSampleRule();
   
           // Act
           rule.Deactivate();
   
           // Assert
           Assert.False(rule.IsActive);
       }
   
       [Fact]
       public void CreateAutoTransaction_WhenInactive_ShouldThrowInvalidOperationException()
       {
           // Arrange
           var rule = CreateSampleRule();
           rule.Deactivate();
   
           // Act & Assert
           var ex = Assert.Throws<InvalidOperationException>(() => rule.CreateAutoTransaction());
           Assert.Equal("Cannot create auto transaction for inactive recurring rule.", ex.Message);
       }
   
    private RecurringRule CreateSampleRule()
    {
        return RecurringRule.Create(
            "Подписка",
            9.99m,
            Currency.USD.Id,
            null,
            TransactionType.Expense.Id,
            RecurringFrequency.Monthly.Id,
            _tomorrow,
            _accountId);
    }
}