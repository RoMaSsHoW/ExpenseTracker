using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public record CreateTransactionCommand(TransactionCreateDTO TransactionDto) 
    : ICommand<TransactionViewDTO>;