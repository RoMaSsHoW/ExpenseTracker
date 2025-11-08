using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public class EditTransactionCommandHandler : ICommandHandler<EditTransactionCommand, TransactionViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditTransactionCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionViewDTO> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request.TransactionDto;
        
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);
        var transaction = account.Transactions.FirstOrDefault(t => t.Id == transactionDto.Id);
        if (transaction is null)
            throw new KeyNotFoundException($"Transaction with ID '{transactionDto.Id}' was not found in the default account.");
        
        transaction.Rename(transactionDto.Name);
        transaction.ChangeCategory(transactionDto.CategoryId);
        transaction.UpdateDescription(transactionDto.Description);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new TransactionViewDTO(transaction);
    }
}