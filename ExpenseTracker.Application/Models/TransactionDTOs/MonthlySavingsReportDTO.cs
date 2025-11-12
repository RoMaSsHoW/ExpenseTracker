namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class MonthlySavingsReportDTO
{
    public DateTime Month { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Savings { get; set; }
}