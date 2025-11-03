using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.UserAggregate.ValueObjects;

public class RefreshToken : ValueObject
{
    public RefreshToken() { }

    public RefreshToken(string token, DateTime expireDate)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token), "Refresh token cannot be null or empty");
        if (expireDate < DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(expireDate), "Refresh token has already expired");

        Token = token;
        ExpireDate = expireDate;
    }

    public string Token { get; }
    public DateTime ExpireDate { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Token;
        yield return ExpireDate;
    }
}