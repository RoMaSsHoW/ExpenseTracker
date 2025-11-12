using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.RecurringRuleCommands;

public class EditRecurringRuleCommandHandler : ICommandHandler<EditRecurringRuleCommand, RecurringRuleViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditRecurringRuleCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RecurringRuleViewDTO> Handle(EditRecurringRuleCommand request, CancellationToken cancellationToken)
    {
        var recurringRuleDto = request.RecurringRuleDto;
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);
        
        var recurringRule = account.RecurringRules.FirstOrDefault(rr => rr.Id == recurringRuleDto.Id);
        if (recurringRule is null)
            throw new KeyNotFoundException($"Recurring Rule with ID '{recurringRuleDto.Id}' was not found in the default account.");
        
        if(recurringRuleDto.IsActive)
            recurringRule.Activate();
        else
            recurringRule.Deactivate();
        
        recurringRule.Rename(recurringRuleDto.Name);
        recurringRule.ChangeAmount(recurringRuleDto.Amount);
        recurringRule.ChangeCategory(recurringRuleDto.CategoryId);
        recurringRule.ChangeType(recurringRuleDto.TypeId);
        recurringRule.ChangeFrequency(recurringRuleDto.RecurringFrequencyId);
        recurringRule.ChangeNextRunAt(recurringRuleDto.NextRunAt);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new RecurringRuleViewDTO(recurringRule);
    }
}