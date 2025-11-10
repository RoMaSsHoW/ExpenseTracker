using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;

namespace ExpenseTracker.Application.Commands.CategoryCommands;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        foreach (var categoryId in request.CategoryDto.Ids)
            _categoryRepository.Remove(categoryId);
        
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}