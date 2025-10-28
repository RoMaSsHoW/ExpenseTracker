using System.Runtime.InteropServices.JavaScript;
using ExpenseTracker.Domain.Common.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Domain.TransactionAggregate.ValueObjects;

namespace ExpenseTracker.Domain.TransactionAggregate;

public class Transaction : Entity
{
    protected Transaction()
    {
        
    }
    
    public string Name { get; private set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public TransactionSource TransactionSource { get; private set; }
    public Guid AccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsRecurring  { get; private set; }
    public Category? Category { get; private set; }
}