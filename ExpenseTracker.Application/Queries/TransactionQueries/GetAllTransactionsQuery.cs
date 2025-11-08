using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public record GetAllTransactionsQuery(TransactionFilterForGetAll Filter) 
    : IQuery<IEnumerable<TransactionViewDTO>>;