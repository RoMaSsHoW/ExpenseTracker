using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class User : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime RefreshTokenExpiryDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
}