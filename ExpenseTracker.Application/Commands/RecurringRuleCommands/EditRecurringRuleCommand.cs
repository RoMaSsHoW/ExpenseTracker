using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;

namespace ExpenseTracker.Application.Commands.RecurringRuleCommands;

public record EditRecurringRuleCommand(RecurringRuleEditDTO RecurringRuleDto) 
    : ICommand<RecurringRuleViewDTO>;
