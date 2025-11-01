using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.ProfileAggregate.ValueObjects;

public class Email : ValueObject
{
    public Email() { }
    
    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentNullException(nameof(address), "Email address cannot be null or empty");
        if (!address.Contains("@"))
            throw new ArgumentOutOfRangeException(nameof(address), "Invalid email address");
        
        Address = address.Trim().ToLower();
        IsConfirmed = false;
    }
    
    public string Address { get; }
    public bool IsConfirmed { get; }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Address;
        yield return IsConfirmed;
    }
}