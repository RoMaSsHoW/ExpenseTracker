using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;
using ExpenseTracker.Domain.UserAggregate.Events;
using MassTransit;

namespace ExpenseTracker.Application.Events;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegistered>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegisteredEventHandler(
        IAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        var userId = context.Message.UserId;

        await CreateDefaultAccountsAsync(userId);

        await CreateDefaultCategoriesAsync(userId);

        await _unitOfWork.CommitAsync();
    }
    
    private async Task CreateDefaultAccountsAsync(Guid userId)
    {
        var existingAccounts = await _accountRepository.FindAllByUserIdAsync(userId);
        var accountsList = existingAccounts.ToList();

        var cashAccount = Account.Create(
            accountsList,
            "Наличные",
            0,
            Currency.UZS.Id,
            userId,
            requestedAsDefault: true);
        
        await _accountRepository.AddAsync(cashAccount);
        accountsList.Add(cashAccount);
        
        var visaAccount = Account.Create(
            accountsList,
            "Карта VISA",
            0,
            Currency.USD.Id,
            userId,
            requestedAsDefault: false);
        
        await _accountRepository.AddAsync(visaAccount);
    }
    
    private async Task CreateDefaultCategoriesAsync(Guid userId)
    {
        var defaultCategories = new[]
        {
            Category.CreateDefault("Еда", TransactionType.Expense.Id, userId),
            Category.CreateDefault("Транстпорт", TransactionType.Expense.Id, userId),
            Category.CreateDefault("Аренда", TransactionType.Expense.Id, userId),
            Category.CreateDefault("Комунальные услуги", TransactionType.Expense.Id, userId),
            Category.CreateDefault("Зарплата", TransactionType.Income.Id, userId)
        };

        foreach (var category in defaultCategories)
            await _categoryRepository.AddAsync(category);
    }
}