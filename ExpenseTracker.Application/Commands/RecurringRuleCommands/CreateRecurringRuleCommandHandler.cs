using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.RecurringRuleCommands;

public class CreateRecurringRuleCommandHandler : ICommandHandler<CreateRecurringRuleCommand, RecurringRuleViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRecurringRuleCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RecurringRuleViewDTO> Handle(CreateRecurringRuleCommand request, CancellationToken cancellationToken)
    {
        var recurringRuleDto = request.RecurringRuleDto;
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);

        var addedRecurringRule = account.AddRecurringRule(
            recurringRuleDto.Name,
            recurringRuleDto.Amount,
            recurringRuleDto.CategoryId,
            recurringRuleDto.TransactionTypeId,
            recurringRuleDto.RecurringFrequencyId,
            recurringRuleDto.StartDate);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new RecurringRuleViewDTO(addedRecurringRule);
    }
}