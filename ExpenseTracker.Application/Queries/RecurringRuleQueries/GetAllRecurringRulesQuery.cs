using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;

namespace ExpenseTracker.Application.Queries.RecurringRuleQueries;

public record GetAllRecurringRulesQuery(FilterForGetAllRecurringRule Filter) 
    : IQuery<PaginatedRecurringRulesDTO>;
