using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, TransactionViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionViewDTO> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request.TransactionDto;
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);
        
        var addedTransaction = account.AddTransaction(
            transactionDto.Name,
            transactionDto.Amount,
            transactionDto.CurrencyId,
            transactionDto.TransactionTypeId,
            TransactionSource.Manual.Id,
            transactionDto.Date,
            transactionDto.Description,
            transactionDto.CategoryId);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new TransactionViewDTO(addedTransaction);
    }
}