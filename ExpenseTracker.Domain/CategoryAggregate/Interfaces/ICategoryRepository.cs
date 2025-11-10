namespace ExpenseTracker.Domain.CategoryAggregate.Interfaces;

public interface ICategoryRepository
{
    Task<Category> FindByIdAsync(Guid categoryId);
    Task AddAsync(Category category);
    void Remove(Guid categoryId);
}