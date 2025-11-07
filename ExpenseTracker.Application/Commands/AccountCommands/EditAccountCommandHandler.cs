using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.AccountDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.AccountCommands;

public class EditAccountCommandHandler : ICommandHandler<EditAccountCommand, AccountViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditAccountCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountViewDTO> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");
        
        var existingAccounts = await _accountRepository.FindAllByUserIdAsync(userId);
        var currentAccount = existingAccounts.FirstOrDefault(a => a.Id == request.AccountDto.Id);
        if (currentAccount is null)
            throw new KeyNotFoundException($"Account with ID {request.AccountDto.Id} was not found.");
        
        currentAccount.Rename(request.AccountDto.Name);
        if (request.AccountDto.IsDefault)
            currentAccount.SetAsDefault(existingAccounts);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new  AccountViewDTO(currentAccount);
    }
}