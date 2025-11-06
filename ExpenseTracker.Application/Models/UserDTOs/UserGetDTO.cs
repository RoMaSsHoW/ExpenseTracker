namespace ExpenseTracker.Application.Models.UserDTOs;

public class UserGetDTO
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; } 
    public string EmailAddress { get; init; } 
    public string EmailIsConfirmed { get; init; }
    public string RoleName { get; init; }
}