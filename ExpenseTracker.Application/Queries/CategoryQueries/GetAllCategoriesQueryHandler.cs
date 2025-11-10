using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Queries.CategoryQueries;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryViewDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetAllCategoriesQueryHandler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<IEnumerable<CategoryViewDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");
        
        var sql = @"
            SELECT 
                id AS Id,
                name AS Name,
                type AS TypeName
            FROM categories
            WHERE user_id = @UserId
            ORDER BY name";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        
        var result = await _dbConnection.QueryAsync<CategoryGetDTO>(sql, parameters);

        return result.Select(c => new CategoryViewDTO(c));
    }
}