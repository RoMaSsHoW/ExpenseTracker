namespace ExpenseTracker.Domain.SeedWork;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}