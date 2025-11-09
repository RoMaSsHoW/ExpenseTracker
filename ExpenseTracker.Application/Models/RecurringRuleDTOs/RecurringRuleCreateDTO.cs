namespace ExpenseTracker.Application.Models.RecurringRuleDTOs;

public class RecurringRuleCreateDTO
{
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public Guid? CategoryId { get; init; }
    public int TransactionTypeId { get; init; }
    public int RecurringFrequencyId { get; init; }
    public DateTime StartDate { get; init; }
}