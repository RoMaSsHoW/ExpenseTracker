namespace ExpenseTracker.Application.Models.RecurringRuleDTOs;

public class RecurringRuleEditDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public Guid? CategoryId { get; init; }
    public int TypeId { get; init; }
    public int RecurringFrequencyId { get; init; }
    public DateTime NextRunAt { get; init; }
    public bool IsActive { get; init; }
}