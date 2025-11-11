namespace ExpenseTracker.Domain.CategoryAggregate.Interfaces;

public interface ICategoryRepository
{
    Task<Category> FindByIdAsync(Guid categoryId);
    Task<IEnumerable<Category>> FindByNamesAsync(List<string> categoryNames);
    Task AddAsync(Category category);
    void Remove(Guid categoryId);
}