using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.TransactionCommands;

public class CreateTransactionsFromFileCommandHandler : ICommandHandler<CreateTransactionsFromFileCommand>
{
    private readonly IHttpAccessor _accessor;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDocumentService _documentService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionsFromFileCommandHandler(
        IHttpAccessor accessor,
        IAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IDocumentService documentService,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _documentService = documentService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTransactionsFromFileCommand request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var account = await _accountRepository.FindDefaultByUserIdAsync(userId);
        
        var transactionsFromFile = _documentService.ReadAsync(request.File);
        
        var categoryNames = transactionsFromFile
            .Where(t => !string.IsNullOrWhiteSpace(t.CategoryName))
            .Select(t => t.CategoryName)
            .Distinct()
            .ToList();

        var categories = await _categoryRepository.FindByNamesAsync(categoryNames);
        var categoryDic = categories.ToDictionary(c => c.Name.ToLower(), c => c.Id);

        foreach (var transactionFromFile in transactionsFromFile)
        {
            categoryDic.TryGetValue(transactionFromFile.CategoryName.ToLower(), out var categoryId);

            var addedTransaction = account.AddTransaction(
                transactionFromFile.Name,
                transactionFromFile.Amount,
                transactionFromFile.TypeId,
                TransactionSource.Imported.Id,
                transactionFromFile.Date,
                transactionFromFile.Description,
                categoryId == Guid.Empty ? null : categoryId);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}