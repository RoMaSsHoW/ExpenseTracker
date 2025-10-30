using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class User : Entity
{
    public User() { }
    
    private User(
        string firstName,
        string lastName,
        string email,
        string password,
        int roleId,
        RefreshToken refreshToken)
    {
        FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.Trim().ToLower());
        LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.Trim().ToLower());
        Email = new Email(email);
        EmailIsConfirmed = false;
        Password = new Password(password);
        Role = Enumeration.FromId<Role>(roleId);
        RefreshToken = refreshToken;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public bool EmailIsConfirmed { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public RefreshToken RefreshToken { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static User Registration(
        string firstName,
        string lastName,
        string email,
        string password,
        int roleId,
        RefreshToken refreshToken)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
        
        return new User(
            firstName, 
            lastName, 
            email,
            password,
            roleId,
            refreshToken);
    }
    
    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password.PasswordHash);
    }
    
    public void ChangeRefreshToken(
        string refreshToken,
        DateTime refreshTokenExpiryDate)
    {
        RefreshToken = new RefreshToken(refreshToken, refreshTokenExpiryDate);
    }

    public void ChangeProfile(
        string firstName,
        string lastName,
        string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));

        FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.Trim().ToLower());
        LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.Trim().ToLower());
        Email = new Email(email);
    }

    public void ChangePassword(string oldPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(oldPassword))
            throw new ArgumentException("Old password cannot be null or whitespace.", nameof(oldPassword));

        if (Verify(oldPassword))
        {
            Password = new Password(newPassword);
        }
    }
}