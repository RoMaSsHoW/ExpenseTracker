using System.Globalization;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate;

public class User : Entity
{
    protected User(
        string firstName,
        string lastName,
        string email,
        string password,
        int roleId,
        string refreshToken,
        DateTime refreshTokenExpiryDate)
    {
        FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.Trim().ToLower());
        LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.Trim().ToLower());
        Email = new Email(email);
        EmailIsConfirmed = false;
        Password = new Password(password);
        Role = Enumeration.FromId<Role>(roleId);
        RefreshToken = refreshToken;
        RefreshTokenExpiryDate =  refreshTokenExpiryDate;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public bool EmailIsConfirmed { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime RefreshTokenExpiryDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static User Registration(
        string firstName,
        string lastName,
        string email,
        string password,
        int roleId,
        string refreshToken,
        DateTime refreshTokenExpiryDate)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or whitespace.", nameof(refreshToken));
        if (refreshTokenExpiryDate < DateTime.UtcNow)
            throw new ArgumentException("Refresh token has already expired.");
        
        return new User(
            firstName, 
            lastName, 
            email,
            password,
            roleId,
            refreshToken,
            refreshTokenExpiryDate);
    }
    
    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password.PasswordHash);
    }
    
    public void ChangeRefreshToken(
        string refreshToken,
        DateTime refreshTokenExpiryDate)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("RefreshToken cannot be null or whitespace.");
        if (RefreshToken ==  refreshToken)
            throw new ArgumentException("RefreshToken has already been changed.");
        if (refreshTokenExpiryDate < DateTime.UtcNow)
            throw new ArgumentException("Refresh token has already expired.");
        
        RefreshToken = refreshToken;
        RefreshTokenExpiryDate = refreshTokenExpiryDate;
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