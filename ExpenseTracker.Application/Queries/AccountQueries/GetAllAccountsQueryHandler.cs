using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.AccountDTOs;

namespace ExpenseTracker.Application.Queries.AccountQueries;

public class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, IEnumerable<AccountViewDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllAccountsQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<AccountViewDTO>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sql = @"
            SELECT 
                id AS Id,
                name AS Name,
                balance AS Balance,
                currency AS CurrencyName,
                is_default AS IsDefault
            FROM accounts
            WHERE user_id = @UserId
            ORDER BY name";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        
        var result = await _dbConnection.QueryAsync<AccountGetDTO>(sql, parameters);
        if (result is null)
            throw new KeyNotFoundException("User not found.");

        return result.Select(account => new AccountViewDTO(account));
    }
}