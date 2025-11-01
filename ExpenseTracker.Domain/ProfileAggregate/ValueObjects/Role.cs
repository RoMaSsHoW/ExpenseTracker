using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.ProfileAggregate.ValueObjects;

public class Role : Enumeration
{
    public static readonly Role Admin = new Role(1, nameof(Admin));
    public static readonly Role User = new Role(2, nameof(User));
    
    public Role(int id, string name) 
        : base(id, name)
    { }
}