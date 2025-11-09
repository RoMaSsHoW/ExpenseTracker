using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Application.Models.TransactionDTOs;

public class TransactionViewDTO
{
    public TransactionViewDTO(Transaction transaction)
    {
        Id =  transaction.Id;
        Name = transaction.Name;
        Amount = transaction.Amount;
        Currency = transaction.Currency;
        Description = transaction.Description;
        CategoryId = transaction.CategoryId;
        Type = transaction.Type;
        AccountId = transaction.AccountId;
        Date = transaction.Date;
    }
    
    public TransactionViewDTO(TransactionGetDTO transaction)
    {
        Id = transaction.Id;
        Name = transaction.Name;
        Amount = transaction.Amount;
        Currency = Enumeration.FromName<Currency>(transaction.CurrencyName);
        Description = transaction.Description;
        CategoryId = transaction.CategoryId;
        CategoryName = transaction.CategoryName;
        Type = Enumeration.FromName<TransactionType>(transaction.TypeName);
        AccountId = transaction.AccountId;
        Date = transaction.Date;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    public string? Description { get; init; }
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public TransactionType Type { get; init; }
    public Guid AccountId { get; init; }
    public DateTime Date { get; init; }
}