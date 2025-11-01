using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.AuthDTOs;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public record LoginCommand(LoginDto Login) : ICommand<AuthResponse>;