using System.Globalization;
using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.UserAggregate.Events;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;

namespace ExpenseTracker.Domain.UserAggregate;

public class User : Entity
{
    public User() { }
    
    private User(
        string firstName,
        string lastName,
        Email email,
        Password password,
        Role role,
        RefreshToken refreshToken)
    {
        FirstName = FormatName(firstName);
        LastName = FormatName(lastName);
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Role = role ?? throw new ArgumentNullException(nameof(role));
        RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        CreatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new UserRegistered(Id));
    }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public RefreshToken RefreshToken { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public static User Register(
        string firstName,
        string lastName,
        string email,
        string password,
        int roleId,
        RefreshToken refreshToken)
    {
        ValidateRegistrationParameters(firstName, lastName);
        
        var emailValueObject = new Email(email);
        var passwordValueObject = new Password(password);
        var role = Enumeration.FromId<Role>(roleId);
        
        return new User(
            firstName, 
            lastName, 
            emailValueObject,
            passwordValueObject,
            role,
            refreshToken);
    }
    
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password.PasswordHash);
    }
    
    public void UpdateRefreshToken(RefreshToken newRefreshToken)
    {
        ArgumentNullException.ThrowIfNull(newRefreshToken);

        RefreshToken = newRefreshToken;
    }

    public void ChangeProfileInfo(
        string firstName,
        string lastName,
        string email)
    {
        ValidateProfileParameters(firstName, lastName);
            
        FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.Trim().ToLower());
        LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.Trim().ToLower());
        Email = new Email(email);
    }

    public void ChangePassword(string oldPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(oldPassword))
            throw new ArgumentException("Old password cannot be null or whitespace.", nameof(oldPassword));

        if (!VerifyPassword(oldPassword))
            throw new UnauthorizedAccessException("Old password is incorrect.");
        
        Password = new Password(newPassword);
    }
    
    private static string FormatName(string name) =>
        CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLower());
    
    private static void ValidateRegistrationParameters(
        string firstName, 
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
    }
    
    private static void ValidateProfileParameters(
        string firstName, 
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
    }
}