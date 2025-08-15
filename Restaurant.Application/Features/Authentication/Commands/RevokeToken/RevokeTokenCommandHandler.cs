using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Authentication.Commands.RevokeToken;

internal sealed class RevokeTokenCommandHandler(IAuthService authService) : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        await authService.RevokeToken(request);
    }
}