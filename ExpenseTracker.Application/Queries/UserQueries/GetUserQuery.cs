using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.UserDTOs;

namespace ExpenseTracker.Application.Queries.UserQueries;

public record GetUserQuery() : IQuery<UserViewDTO>;