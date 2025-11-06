using System.Data;
using Dapper;
using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.UserDTOs;

namespace ExpenseTracker.Application.Queries.UserQueries;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserGetDTO>
{
    private readonly IDbConnection _dbConnection;
    private readonly IHttpAccessor _accessor;

    public GetUserQueryHandler(IDbConnection dbConnection, IHttpAccessor accessor)
    {
        _dbConnection = dbConnection;
        _accessor = accessor;
    }

    public async Task<UserGetDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var sql = @"
            SELECT 
                id as Id,
                first_name as FirstName,
                last_name as LastName,
                email_address as EmailAddress,
                email_is_confirmed as EmailIsConfirmed,
                role as RoleName
            FROM users 
            WHERE Id = @Id";
                  
        var parameters = new DynamicParameters();
        parameters.Add("@Id", userId, DbType.Guid);
        
        var result = await _dbConnection.QueryFirstOrDefaultAsync<UserGetDTO>(sql, parameters);
        if (result is null)
            throw new KeyNotFoundException("User not found.");

        return result;
    }
}