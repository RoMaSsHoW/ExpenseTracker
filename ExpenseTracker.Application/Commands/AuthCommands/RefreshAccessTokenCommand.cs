using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public record RefreshAccessTokenCommand(string RefreshToken) : ICommand<AuthResponse>;