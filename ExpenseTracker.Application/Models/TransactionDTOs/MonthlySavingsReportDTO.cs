namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class MonthlySavingsReportDTO
{
    public DateTime Month { get; init; }
    public decimal Income { get; init; }
    public decimal Expense { get; init; }
    public decimal Savings { get; init; }
}