using ExpenseTracker.Domain.UserAggregate;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;

namespace ExpenseTracker.Application.Models.UserDTOs;

public class UserGetDTO
{
    public UserGetDTO(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Role = user.Role;
    }
    
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; } 
    public Email Email { get; init; } 
    public Role Role { get; init; }
}