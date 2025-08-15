using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(IAuthService authService) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var response = await authService.RefreshToken(request);

        return response;
    }
}