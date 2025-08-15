using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(IAuthService authService,
    ILogger<Features.Authentication.Commands.Register.RegisterCommand> loggger) : IRequestHandler<Features.Authentication.Commands.Register.RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(Features.Authentication.Commands.Register.RegisterCommand request, CancellationToken cancellationToken)
    {
        loggger.LogInformation("Register a user.");

        var authResponseDto = await authService.RegisterAsync(request);

        return authResponseDto;
    }
}