using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public record EditTransactionCommand(TransactionEditDTO TransactionDto) 
    : ICommand<TransactionViewDTO>;