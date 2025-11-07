using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.AccountDTOs;

namespace ExpenseTracker.Application.Commands.AccountCommands;

public record EditAccountCommand(AccountEditDTO AccountDto) 
    : ICommand<AccountViewDTO>;