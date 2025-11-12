using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Queries.CategoryQueries;

public class GetTopCategoriesQueryHanler : IQueryHandler<GetTopCategoriesQuery, IEnumerable<TopCategoryDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetTopCategoriesQueryHanler(
        IDbConnection dbConnection,
        IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public Task<IEnumerable<TopCategoryDTO>> Handle(GetTopCategoriesQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sql = @"
            SELECT
                c.name AS CategoryName,
                SUM(t.amount) AS TotalSpent,
                COUNT(*) AS TransactionCount
            FROM transactions t
            INNER JOIN categories c ON c.id = t.category_id
            INNER JOIN accounts a ON a.id = t.account_id
            WHERE t.type = 'Expense' 
            AND a.user_id = @UserId
            GROUP BY c.name
            ORDER BY TotalSpent DESC
            LIMIT 5;";
        
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        
        var  result = _dbConnection.QueryAsync<TopCategoryDTO>(sql, parameters);
        
        return result;
    }
}