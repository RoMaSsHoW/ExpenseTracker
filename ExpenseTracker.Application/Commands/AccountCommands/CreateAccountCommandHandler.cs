using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.AccountDTOs;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.AccountCommands;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, AccountViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountViewDTO> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var existingAccounts = await _accountRepository.FindAllByUserIdAsync(userId);
        var newAccountDto = request.AccountDto;

        var account = Account.Create(
            existingAccounts.ToList(),
            newAccountDto.Name,
            newAccountDto.Balance,
            newAccountDto.CurrencyId,
            userId,
            newAccountDto.IsDefault);

        await _accountRepository.AddAsync(account);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AccountViewDTO(account);
    }
}