using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

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

        var sql = @"
            SELECT
                t.id AS Id,
                t.name AS Name,
                t.amount AS Amount,
                t.currency AS CurrencyName,
                t.description AS Description,
                t.category_id AS CategoryId,
                t.type AS TypeName,
                t.account_id AS AccountId,
                t.date AS Date
            FROM transactions t
            INNER JOIN accounts a ON t.account_id = a.id AND a.is_default = TRUE
            WHERE a.user_Id = @UserId";
                
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        if (!string.IsNullOrWhiteSpace(request.Filter.Name))
        {
            sql += " AND name ILIKE @Name";
            parameters.Add("Name", $"{request.Filter.Name}%");
        }
        
        if (request.Filter?.AmountFrom is not null)
        {
            sql += " AND amount >= @AmountFrom";
            parameters.Add("AmountFrom", request.Filter.AmountFrom);
        }

        if (request.Filter?.AmountTo is not null)
        {
            sql += " AND amount <= @AmountTo";
            parameters.Add("AmountTo", request.Filter.AmountTo);
        }
        
        if (request.Filter?.CategoryId is not null)
        {
            sql += " AND category_id = @CategoryId";
            parameters.Add("CategoryId", request.Filter.CategoryId);
        }
        
        if (request.Filter?.DateFrom is not null)
        {
            sql += " AND date >= @DateFrom";
            parameters.Add("DateFrom", request.Filter.DateFrom);
        }

        if (request.Filter?.DateTo is not null)
        {
            sql += " AND date <= @DateTo";
            parameters.Add("DateTo", request.Filter.DateTo);
        }
        else
        {
            sql += " AND date <= @DateTo";
            parameters.Add("DateTo", DateTime.UtcNow);
        }
        
        sql += " ORDER BY date DESC";
        
        var transactions = await _dbConnection.QueryAsync<TransactionGetDTO>(sql, parameters);

        return transactions.Select(t => new TransactionViewDTO(t));
    }
}