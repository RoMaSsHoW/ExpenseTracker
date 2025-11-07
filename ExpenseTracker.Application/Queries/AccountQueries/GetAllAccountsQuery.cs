using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.AccountDTOs;

namespace ExpenseTracker.Application.Queries.AccountQueries;

public record GetAllAccountsQuery() : IQuery<IEnumerable<AccountViewDTO>>;