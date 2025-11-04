using MediatR;

namespace ExpenseTracker.Application.Common.Messaging;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{ }