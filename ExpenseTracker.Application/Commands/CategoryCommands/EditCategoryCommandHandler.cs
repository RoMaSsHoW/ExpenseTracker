using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.CategoryCommands;

public class EditCategoryCommandHandler : ICommandHandler<EditCategoryCommand, CategoryViewDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryViewDTO> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryDto = request.CategoryDto;

        var category = await _categoryRepository.FindByIdAsync(categoryDto.Id);
        
        category.Rename(categoryDto.Name);
        category.ChangeType(categoryDto.TypeId);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return new CategoryViewDTO(category);
    }
}