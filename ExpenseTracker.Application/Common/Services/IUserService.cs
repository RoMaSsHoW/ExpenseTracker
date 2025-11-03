using ExpenseTracker.Application.Models.UserDTOs;

namespace ExpenseTracker.Application.Common.Services;

public interface IUserService
{
    Task<UserGetDTO> GetUserAsync();    
}