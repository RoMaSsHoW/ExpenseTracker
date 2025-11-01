using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Models.UserDTOs;
using ExpenseTracker.Domain.UserAggregate.Interfaces;

namespace ExpenseTracker.Application.Services;

public class UserService : IUserService
{
    private readonly IHttpAccessor _accessor;
    private readonly IUserRepository _userRepository;

    public UserService(
        IHttpAccessor accessor,
        IUserRepository userRepository)
    {
        _accessor = accessor;
        _userRepository = userRepository;
    }

    public async Task<UserGetDTO> GetUserAsync()
    {
        var userId = _accessor.GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await _userRepository.FindById(userId);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return new UserGetDTO(user);
    }
}