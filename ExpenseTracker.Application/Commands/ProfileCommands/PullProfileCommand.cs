using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.ProfileDTOs;

namespace ExpenseTracker.Application.Commands.ProfileCommands;

public record PullProfileCommand() : ICommand<UserGetDTO>;