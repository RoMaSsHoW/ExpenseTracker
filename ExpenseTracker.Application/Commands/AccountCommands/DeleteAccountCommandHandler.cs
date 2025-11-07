using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.AccountCommands;

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork, 
        IHttpAccessor accessor)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");
        
        var existingAccounts = await _accountRepository.FindAllByUserIdAsync(userId);
        var currentAccount = existingAccounts.FirstOrDefault(a => a.Id == request.AccountDto.Id);
        if (currentAccount is null)
            throw new KeyNotFoundException($"Account with ID {request.AccountDto.Id} was not found.");

        if (currentAccount.IsDefault && existingAccounts.Count() > 1)
        {
            var newDefault = existingAccounts
                .FirstOrDefault(a => a.Id != currentAccount.Id);

            if (newDefault is not null)
                newDefault.SetAsDefault(existingAccounts);
        }
        
        _accountRepository.Remove(request.AccountDto.Id);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}