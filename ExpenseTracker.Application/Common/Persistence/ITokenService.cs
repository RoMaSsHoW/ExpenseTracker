using ExpenseTracker.Domain.UserAggregate;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;

namespace ExpenseTracker.Application.Common.Persistence
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        RefreshToken GenerateRefreshToken();

        bool IsRefreshTokenExpired(User user);
    }
}
