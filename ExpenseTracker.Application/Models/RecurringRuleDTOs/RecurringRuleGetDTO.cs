namespace ExpenseTracker.Application.Models.RecurringRuleDTOs;

public class RecurringRuleGetDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public string CurrencyName { get; init; }
    public Guid? CategoryId { get; init; }
    public string TypeName { get; init; }
    public string FrequencyName { get; init; }
    public DateTime NextRunAt { get; init; }
    public bool IsActive { get; init; }
    public Guid AccountId { get; init; }
}