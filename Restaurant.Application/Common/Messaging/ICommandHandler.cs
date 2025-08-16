using MediatR;

namespace Restaurant.Application.Common.messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
where TResponse : notnull
where TCommand : ICommand<TResponse>
{ }

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit>
where TCommand : ICommand;