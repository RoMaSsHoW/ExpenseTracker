using System.Security.Claims;

namespace ExpenseTracker.Application.Common.Persistence;

public interface IHttpAccessor
{
    public ClaimsPrincipal? User { get; }
    
    Guid? GetUserId();
}