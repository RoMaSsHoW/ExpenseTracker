using ExpenseTracker.Application.Models.UserDTOs;

namespace ExpenseTracker.Application.Services;

public interface IUserService
{
    Task<UserGetDTO> GetUserAsync();    
}