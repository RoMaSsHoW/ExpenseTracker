using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Domain.ProfileAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Login.Email);
        if (user is null)
            throw new UnauthorizedAccessException("Such user does not exist");
        
        if (!user.Verify(request.Login.Password))
            throw new UnauthorizedAccessException("Wrong password");

        if (string.IsNullOrEmpty(user.RefreshToken.Token) || _tokenService.IsRefreshTokenExpired(user))
        {
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.ChangeRefreshToken(refreshToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new AuthResponse(accessToken, user.RefreshToken.Token);
    }
}