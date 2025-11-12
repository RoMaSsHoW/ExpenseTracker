using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.UserAggregate.Events;

public class UserRegistered : DomainEventBase
{
    public UserRegistered() { }
    
    public UserRegistered(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; init; }
}