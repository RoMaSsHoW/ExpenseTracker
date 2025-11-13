using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Application.Queries.TransactionQueries;

public class GetAllPaginatedTransactionsQueryHandler : IQueryHandler<GetAllPaginatedTransactionsQuery, PaginatedTransactionsDTO>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllPaginatedTransactionsQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<PaginatedTransactionsDTO> Handle(GetAllPaginatedTransactionsQuery request, CancellationToken cancellationToken)
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

        if (!string.IsNullOrWhiteSpace(request.Filter.Name))
        {
            sqlBase += " AND t.name ILIKE @Name";
            parameters.Add("Name", $"{request.Filter.Name}%");
        }

        if (request.Filter.AmountFrom is not null)
        {
            sqlBase += " AND t.amount >= @AmountFrom";
            parameters.Add("AmountFrom", request.Filter.AmountFrom);
        }

        if (request.Filter.AmountTo is not null)
        {
            sqlBase += " AND t.amount <= @AmountTo";
            parameters.Add("AmountTo", request.Filter.AmountTo);
        }

        if (request.Filter.CategoryId is not null)
        {
            sqlBase += " AND t.category_id = @CategoryId";
            parameters.Add("CategoryId", request.Filter.CategoryId);
        }

        if (request.Filter.DateFrom is not null)
        {
            sqlBase += " AND t.date >= @DateFrom";
            parameters.Add("DateFrom", request.Filter.DateFrom);
        }

        if (request.Filter.DateTo is not null)
        {
            sqlBase += " AND t.date <= @DateTo";
            parameters.Add("DateTo", request.Filter.DateTo);
        }
        // else
        // {
            // sqlBase += " AND t.date <= @DateTo";
            // parameters.Add("DateTo", DateTime.UtcNow);
        // }

        var countSql = $"SELECT COUNT(*) {sqlBase}";
        var totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

        var pageNumber = request.Filter.PageNumber <= 0 ? 1 : request.Filter.PageNumber;
        var pageSize = request.Filter.PageSize <= 0 ? 10 : request.Filter.PageSize;
        var offset = (pageNumber - 1) * pageSize;

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
            ORDER BY t.date DESC
            LIMIT @PageSize OFFSET @Offset";

        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);

        var transactions = await _dbConnection.QueryAsync<TransactionGetDTO>(sql, parameters);

        var viewList = transactions.Select(t => new TransactionViewDTO(t));

        return new PaginatedTransactionsDTO
        {
            Items = viewList,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}