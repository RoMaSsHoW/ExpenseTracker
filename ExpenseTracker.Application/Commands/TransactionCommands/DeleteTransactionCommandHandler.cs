using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public class DeleteTransactionCommandHandler : ICommandHandler<DeleteTransactionCommand>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request.TransactionDto;
        
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);

        foreach (var transactionId in transactionDto.Ids)
            account.RemoveTransaction(transactionId);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}