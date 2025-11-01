using ExpenseTracker.Domain.ProfileAggregate;
using ExpenseTracker.Domain.ProfileAggregate.ValueObjects;

namespace ExpenseTracker.Application.Models.ProfileDTOs;

public class UserGetDTO
{
    public UserGetDTO(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Role = user.Role;
        Accounts = user.Accounts.Select(x => new AccountGetDTO(x)).ToList();
    }
    
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; } 
    public Email Email { get; init; } 
    public Role Role { get; init; }
    public List<AccountGetDTO> Accounts { get; init; }
}