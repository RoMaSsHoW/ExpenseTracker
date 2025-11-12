using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Domain.UserAggregate.Events;
using MassTransit;

namespace ExpenseTracker.Application.Events;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegistered>
{
    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        var userId = context.Message.UserId;
        
    }
}