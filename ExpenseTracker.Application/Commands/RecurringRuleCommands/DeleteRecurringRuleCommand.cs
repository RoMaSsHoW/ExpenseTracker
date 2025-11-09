using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;

namespace ExpenseTracker.Application.Commands.RecurringRuleCommands;

public record DeleteRecurringRuleCommand(RecurringRuleDeleteDTO RecurringRuleDto) 
    : ICommand;