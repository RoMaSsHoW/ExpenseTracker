namespace ExpenseTracker.Application.Models.CategoryDTOs;

public class CategoryCreateDTO
{
    public string Name  { get; init; }
    public int TransactionTypeId { get; init; }
}