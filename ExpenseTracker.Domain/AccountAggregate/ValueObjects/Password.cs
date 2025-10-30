using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class Password : ValueObject
{
    public Password() { }

    public Password(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be empty");
        
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password.Trim());
    }
    
    public string PasswordHash { get; private set; }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PasswordHash;
    }
}