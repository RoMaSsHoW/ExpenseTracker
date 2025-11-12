using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public class GetMonthlySavingQueryHandler : IQueryHandler<GetMonthlySavingQuery, IEnumerable<MonthlySavingsReportDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetMonthlySavingQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<MonthlySavingsReportDTO>> Handle(GetMonthlySavingQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sql = @"
            SELECT
                income.Month,
                income.total AS Income,
                expense.total AS Expense,
                (income.total - expense.total) AS Savings
            FROM (
                    SELECT DATE_TRUNC('month', t.date) AS Month, SUM(t.amount) AS total
                    FROM transactions t
                    INNER JOIN accounts a ON a.id = t.account_id
                    WHERE t.type = 'Income' AND a.user_id = @UserId
                    GROUP BY Month) income
            JOIN (
                    SELECT DATE_TRUNC('month', t.date) AS Month, SUM(t.amount) AS total
                    FROM transactions t
                    INNER JOIN accounts a ON a.id = t.account_id
                    WHERE t.type = 'Expense' AND a.user_id = @UserId
                    GROUP BY Month) expense
            ON income.Month = expense.Month
            ORDER BY income.Month DESC";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        
        var result = await _dbConnection.QueryAsync<MonthlySavingsReportDTO>(sql, parameters);
        
        return result ?? throw new KeyNotFoundException("User not found.");
    }
}