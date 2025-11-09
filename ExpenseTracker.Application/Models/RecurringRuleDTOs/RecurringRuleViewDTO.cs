using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Application.Models.RecurringRuleDTOs;

public class RecurringRuleViewDTO
{
    public RecurringRuleViewDTO(RecurringRule recurringRule)
    {
        Id = recurringRule.Id;
        Name = recurringRule.Name;
        Amount = recurringRule.Amount;
        Currency = recurringRule.Currency;
        CategoryId = recurringRule.CategoryId;
        Type = recurringRule.Type;
        Frequency = recurringRule.Frequency;
        NextRunAt = recurringRule.NextRunAt;
        IsActive = recurringRule.IsActive;
        AccountId = recurringRule.AccountId;
    }

    public RecurringRuleViewDTO(RecurringRuleGetDTO recurringRule)
    {
        Id = recurringRule.Id;
        Name = recurringRule.Name;
        Amount = recurringRule.Amount;
        Currency = Enumeration.FromName<Currency>(recurringRule.CurrencyName);
        CategoryId = recurringRule.CategoryId;
        Type = Enumeration.FromName<TransactionType>(recurringRule.TypeName);
        Frequency = Enumeration.FromName<RecurringFrequency>(recurringRule.FrequencyName);
        NextRunAt = recurringRule.NextRunAt;
        IsActive = recurringRule.IsActive;
        AccountId = recurringRule.AccountId;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    public Guid? CategoryId { get; init; }
    public TransactionType Type { get; init; }
    public RecurringFrequency Frequency { get; init; }
    public DateTime NextRunAt { get; init; }
    public bool IsActive { get; init; }
    public Guid AccountId { get; init; }
}
