using ExpenseTracker.Domain.SeedWork;
using MassTransit;

namespace ExpenseTracker.Application.Common.Messaging;

public interface IDomainEventHandler<in TEvent> : IConsumer<TEvent>
    where TEvent : class, IDomainEvent
{ }