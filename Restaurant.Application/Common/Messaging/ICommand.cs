using MediatR;

namespace Restaurant.Application.Common.messaging;

public interface ICommand : IRequest<Unit>
{ }

public interface ICommand<out TResponse> : IRequest<TResponse>
where TResponse : notnull;