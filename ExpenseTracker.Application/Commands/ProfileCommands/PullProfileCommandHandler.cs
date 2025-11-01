using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Models.ProfileDTOs;
using ExpenseTracker.Domain.ProfileAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.ProfileCommands;

public class PullProfileCommandHandler : ICommandHandler<PullProfileCommand, UserGetDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IUserRepository _userRepository;

    public PullProfileCommandHandler(IHttpAccessor accessor, IUserRepository userRepository)
    {
        _accessor = accessor;
        _userRepository = userRepository;
    }

    public async Task<UserGetDTO> Handle(PullProfileCommand request, CancellationToken cancellationToken)
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