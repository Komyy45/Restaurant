using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(Features.Authentication.Commands.Register.RegisterCommand request, CancellationToken cancellationToken)
    {
        var authResponseDto = await authService.RegisterAsync(request);

        return authResponseDto;
    }
}