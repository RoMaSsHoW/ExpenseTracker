using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.RecurringRuleCommands;

public class DeleteRecurringRuleCommandHandler : ICommandHandler<DeleteRecurringRuleCommand>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRecurringRuleCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteRecurringRuleCommand request, CancellationToken cancellationToken)
    {
        var recurringRuleDto = request.RecurringRuleDto;
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);

        foreach (var recurringRuleId in recurringRuleDto.Ids)
            account.RemoveRecurringRule(recurringRuleId);
        
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}