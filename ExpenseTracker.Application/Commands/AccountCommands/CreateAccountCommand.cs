using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.AccountDTOs;

namespace ExpenseTracker.Application.Commands.AccountCommands;

public record CreateAccountCommand(AccountCreateDTO AccountDto) 
    : ICommand<AccountViewDTO>;