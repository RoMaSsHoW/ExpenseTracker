namespace ExpenseTracker.Application.Models.CategoryDTOs;

public class CategoryGetDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string TypeName { get; init; }
}