using ExpenseTracker.Application.Common.Messaging;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public record CreateTransactionsFromFileCommand(IFormFile File) : ICommand;