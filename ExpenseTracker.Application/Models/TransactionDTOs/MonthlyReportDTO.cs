namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class MonthlyReportDTO
{
    public DateTime Month { get; init; }
    public decimal TotalIncomeAmount { get; init; }
    public decimal TotalExpenseAmount { get; init; }
    public int TotalIncomeTransactionCount { get; init; }
    public int TotalExpenseTransactionCount { get; init; }
    public int TotalTransactionCount { get; init; }
}