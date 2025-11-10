using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Queries.CategoryQueries;

public class GetAllCategoriesLikeEnumQueryHandler : IQueryHandler<GetAllCategoriesLikeEnumQuery, IEnumerable<CategoryEnumViewDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllCategoriesLikeEnumQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<CategoryEnumViewDTO>> Handle(GetAllCategoriesLikeEnumQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");
        
        var sql = @"
            SELECT 
                id AS Id,
                name AS Name
            FROM categories
            WHERE user_id = @UserId
            ORDER BY name";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        
        var result = await _dbConnection.QueryAsync<CategoryEnumViewDTO>(sql, parameters);

        return result;
    }
}