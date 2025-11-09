namespace ExpenseTracker.Application.Models.RecurringRuleDTOs;

public class PaginatedRecurringRulesDTO
{
    public IEnumerable<RecurringRuleViewDTO> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}