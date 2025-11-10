using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.CategoryAggregate;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Application.Models.CategoryDTOs;

public class CategoryViewDTO
{
    public CategoryViewDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Type = category.Type;
    }

    public CategoryViewDTO(CategoryGetDTO category)
    {
        Id = category.Id;
        Name = category.Name;
        Type = Enumeration.FromName<TransactionType>(category.TypeName);
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public TransactionType Type { get; init; }
}