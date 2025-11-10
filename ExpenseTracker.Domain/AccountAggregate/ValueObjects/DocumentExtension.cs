using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.AccountAggregate.ValueObjects;

public class DocumentExtension : Enumeration
{
    public static readonly DocumentExtension XLSX = new DocumentExtension(1, nameof(XLSX));
    public static readonly DocumentExtension CSV = new DocumentExtension(2, nameof(CSV));
    
    public DocumentExtension(int id, string name) 
        : base(id, name)
    { }
}