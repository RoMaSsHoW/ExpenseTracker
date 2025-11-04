using MediatR;

namespace ExpenseTracker.Application.Common.Messaging;

public interface IQuery<out TResult> : IRequest<TResult>
{ }