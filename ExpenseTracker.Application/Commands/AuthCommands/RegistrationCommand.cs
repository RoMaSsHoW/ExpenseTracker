using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.AccountDTOs;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public record RegistrationCommand(RegisterDto Register) : ICommand<AuthResponse>;