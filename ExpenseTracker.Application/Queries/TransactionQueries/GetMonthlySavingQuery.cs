using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public record GetMonthlySavingQuery() : IQuery<IEnumerable<MonthlySavingsReportDTO>>;