using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public class GetAllTransactionsQueryHandler : IQueryHandler<GetAllTransactionsQuery, IEnumerable<TransactionViewDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllTransactionsQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<TransactionViewDTO>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sqlBase = @"
            FROM transactions t
            INNER JOIN accounts a ON t.account_id = a.id AND a.is_default = TRUE
            LEFT JOIN categories c ON t.category_id = c.id
            WHERE a.user_id = @UserId";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        if (request.Filter.DateFrom is not null)
        {
            sqlBase += " AND t.date >= @DateFrom";
            parameters.Add("DateFrom", request.Filter.DateFrom);
        }
        else
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            sqlBase += " AND t.date >= @DateFrom";
            parameters.Add("DateFrom", startOfMonth);
        }

        if (request.Filter.DateTo is not null)
        {
            sqlBase += " AND t.date <= @DateTo";
            parameters.Add("DateTo", request.Filter.DateTo);
        }
        else
        {
            sqlBase += " AND t.date <= @DateTo";
            parameters.Add("DateTo", DateTime.UtcNow);
        }
        
        var sql = $@"
            SELECT
                t.id AS Id,
                t.name AS Name,
                t.amount AS Amount,
                t.currency AS CurrencyName,
                t.description AS Description,
                t.category_id AS CategoryId,
                c.name AS CategoryName,
                t.type AS TypeName,
                t.account_id AS AccountId,
                t.date AS Date
            {sqlBase}
            ORDER BY t.date DESC";

        var transactions = await _dbConnection.QueryAsync<TransactionGetDTO>(sql, parameters);

        return transactions.Select(t => new TransactionViewDTO(t));
    }
}