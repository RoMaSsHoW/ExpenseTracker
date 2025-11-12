namespace ExpenseTracker.Application.Models.CategoryDTOs;

public class TopCategoryDTO
{
    public string CategoryName { get; init; }
    public decimal TotalSpent { get; init; }
    public int TransactionCount { get; init; }
}