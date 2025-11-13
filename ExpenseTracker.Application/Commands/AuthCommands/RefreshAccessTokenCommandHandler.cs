using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Domain.UserAggregate;
using ExpenseTracker.Domain.UserAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public class RefreshAccessTokenCommandHandler : ICommandHandler<RefreshAccessTokenCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshAccessTokenCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateUserAsync(request);

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.UpdateRefreshToken(newRefreshToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
            
        return new AuthResponse(accessToken, newRefreshToken.Token);
    }

    private async Task<User> ValidateUserAsync(RefreshAccessTokenCommand request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            throw new UnauthorizedAccessException("Refresh token cannot be empty.");
        
        var user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken);
        if (user is null)
            throw new UnauthorizedAccessException("User associated with this refresh token was not found.");

        if (_tokenService.IsRefreshTokenExpired(user))
            throw new UnauthorizedAccessException("Refresh token has expired. Please log in again.");

        return user;
    }
}