using System.Security.Claims;

namespace ExpenseTracker.Application.Common.Services;

public interface IHttpAccessor
{
    public ClaimsPrincipal? User { get; }
    
    Guid? GetUserId();
}