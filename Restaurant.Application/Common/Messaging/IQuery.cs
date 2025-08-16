using MediatR;

namespace Restaurant.Application.Common.messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
where TResponse : notnull;