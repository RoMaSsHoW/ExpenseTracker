using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;

namespace ExpenseTracker.Application.Models.UserDTOs;

public class UserViewDTO
{
    public UserViewDTO(UserGetDTO user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = new Email(user.EmailAddress);
        Role = Enumeration.FromName<Role>(user.RoleName);
    }
    
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; } 
    public Email Email { get; init; } 
    public Role Role { get; init; }
}