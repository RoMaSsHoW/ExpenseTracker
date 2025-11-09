using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;

namespace ExpenseTracker.Application.Queries.RecurringRuleQueries;

public class GetAllRecurringRulesQueryHandler : IQueryHandler<GetAllRecurringRulesQuery, PaginatedRecurringRulesDTO>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllRecurringRulesQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<PaginatedRecurringRulesDTO> Handle(GetAllRecurringRulesQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sqlBase = @"
            FROM recurring_rules rr
            INNER JOIN accounts a ON rr.account_id = a.id AND a.is_default = TRUE
            WHERE a.user_id = @UserId";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        
        if (!string.IsNullOrWhiteSpace(request.Filter.Name))
        {
            sqlBase += " AND rr.name ILIKE @Name";
            parameters.Add("Name", $"{request.Filter.Name}%");
        }

        if (request.Filter.AmountFrom is not null)
        {
            sqlBase += " AND rr.amount >= @AmountFrom";
            parameters.Add("AmountFrom", request.Filter.AmountFrom);
        }

        if (request.Filter.AmountTo is not null)
        {
            sqlBase += " AND rr.amount <= @AmountTo";
            parameters.Add("AmountTo", request.Filter.AmountTo);
        }

        if (request.Filter.CategoryId is not null)
        {
            sqlBase += " AND rr.category_id = @CategoryId";
            parameters.Add("CategoryId", request.Filter.CategoryId);
        }

        var countSql = $"SELECT COUNT(*) {sqlBase}";
        var totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

        var pageNumber = request.Filter.PageNumber <= 0 ? 1 : request.Filter.PageNumber;
        var pageSize = request.Filter.PageSize <= 0 ? 10 : request.Filter.PageSize;
        var offset = (pageNumber - 1) * pageSize;
        
        var sql = $@"
            SELECT
                rr.id AS Id,
                rr.name AS Name,
                rr.amount AS Amount,
                rr.currency AS CurrencyName,
                rr.category_id AS CategoryId,
                rr.type AS TypeName,
                rr.frequency AS FrequencyName,
                rr.next_run_at AS NextRunAt,
                rr.is_active AS IsActive,
                rr.account_id AS AccountId
            {sqlBase}
            ORDER BY rr.created_at
            LIMIT @PageSize OFFSET @Offset";

        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);
        
        var recurringRules = await _dbConnection.QueryAsync<RecurringRuleGetDTO>(sql, parameters);
        
        var viewList = recurringRules.Select(rr => new RecurringRuleViewDTO(rr));

        return new PaginatedRecurringRulesDTO()
        {
            Items = viewList,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }
}