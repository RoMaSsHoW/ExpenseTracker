using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Domain.UserAggregate;
using ExpenseTracker.Domain.UserAggregate.Interfaces;
using ExpenseTracker.Domain.UserAggregate.ValueObjects;

namespace ExpenseTracker.Application.Commands.AuthCommands;

public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrationCommandHandler(
        IUserRepository userRepository, 
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        var user = User.Registration(
            request.Register.FirstName,
            request.Register.LastName,
            request.Register.Email,
            request.Register.Password,
            Role.User.Id,
            refreshToken);

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        
        return new AuthResponse(accessToken, refreshToken.Token);
    }
}