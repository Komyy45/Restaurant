using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Authentication.Commands.RevokeToken;

internal sealed class RevokeTokenCommandHandler(IAuthService authService,
    ILogger<RevokeTokenCommandHandler> logger) : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling Revoke Token Command.");

        await authService.RevokeToken(request);
    }
}