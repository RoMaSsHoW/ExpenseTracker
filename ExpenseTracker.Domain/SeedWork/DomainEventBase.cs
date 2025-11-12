namespace ExpenseTracker.Domain.SeedWork;

public class DomainEventBase : IDomainEvent
{
    public DomainEventBase()
    {
        OccuredOn = DateTime.UtcNow;
    }

    public DateTime OccuredOn { get; }
}