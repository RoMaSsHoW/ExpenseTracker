namespace ExpenseTracker.Application.Models.CategoryDTOs;

public class CategoryEditDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int TypeId { get; init; }
}