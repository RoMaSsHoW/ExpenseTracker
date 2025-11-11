using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public class GetTotalExpensesAndIncomesPerMonthQueryHandler : IQueryHandler<GetTotalExpensesAndIncomesPerMonthQuery, IEnumerable<MonthlyReportDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetTotalExpensesAndIncomesPerMonthQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<MonthlyReportDTO>> Handle(GetTotalExpensesAndIncomesPerMonthQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sql = @"
            SELECT
                DATE_TRUNC('month', t.date) AS Month,
                SUM(t.amount) FILTER (WHERE t.type = 'Income') AS TotalIncomeAmount,
                SUM(t.amount) FILTER (WHERE t.type = 'Expense') AS TotalExpenseAmount,
                COUNT(*) FILTER (WHERE t.type = 'Income') AS TotalIncomeTransactionCount,
                COUNT(*) FILTER (WHERE t.type = 'Expense') AS TotalExpenseTransactionCount,
                COUNT(*) AS TotalTransactionCount
            FROM transactions t
            INNER JOIN accounts a ON a.id = t.account_id
            WHERE a.user_id = @UserId
            GROUP BY Month
            ORDER BY Month DESC";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);

        var result = await _dbConnection.QueryAsync<MonthlyReportDTO>(sql, parameters);
        
        return result ?? throw new KeyNotFoundException("User not found.");
    }
}