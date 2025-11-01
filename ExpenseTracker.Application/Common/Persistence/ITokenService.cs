using ExpenseTracker.Domain.ProfileAggregate;
using ExpenseTracker.Domain.ProfileAggregate.ValueObjects;

namespace ExpenseTracker.Application.Common.Persistence
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        RefreshToken GenerateRefreshToken();

        bool IsRefreshTokenExpired(User user);
    }
}
