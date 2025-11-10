using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.CategoryCommands;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryViewDTO>
{
    private readonly IHttpAccessor _accessor;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        IHttpAccessor accessor,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryViewDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _accessor.GetUserId() 
                     ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var createdCategory = Category.Create(
            request.CategoryDto.Name,
            request.CategoryDto.TransactionTypeId,
            userId);

        await _categoryRepository.AddAsync(createdCategory);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new CategoryViewDTO(createdCategory);
    }
}