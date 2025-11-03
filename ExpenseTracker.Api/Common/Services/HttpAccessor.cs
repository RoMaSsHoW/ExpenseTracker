using System.Security.Claims;
using ExpenseTracker.Application.Common.Services;

namespace ExpenseTracker.Api.Common.Services;

public class HttpAccessor : IHttpAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    
    public Guid? GetUserId()
    {
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}