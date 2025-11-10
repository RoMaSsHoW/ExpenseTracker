using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.CategoryAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IAppDbContext _dbContext;
    
    public CategoryRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Category> FindByIdAsync(Guid categoryId)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId);
        
        if (category is null)
            throw new KeyNotFoundException($"Category with ID '{categoryId}' was not found.");
        
        return category;
    }

    public async Task<IEnumerable<Category>> FindByIdsAsync(List<Guid> categoryIds)
    {
        if (categoryIds == null || categoryIds.Count == 0)
            return Enumerable.Empty<Category>();

        var categories = await _dbContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        return categories;
    }

    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
    }

    public void Remove(Guid categoryId)
    {
        var category = _dbContext.Categories.Find(categoryId);
        if (category is null)
            throw new KeyNotFoundException($"Category with ID '{categoryId}' was not found.");
        
        _dbContext.Categories.Remove(category);
    }
}