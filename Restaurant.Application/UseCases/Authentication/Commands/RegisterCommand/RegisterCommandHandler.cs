using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;

internal sealed class RegisterCommandHandler(IAuthService authService,
    ILogger<RegisterCommand> loggger) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        loggger.LogInformation("Register a user.");

        var authResponseDto = await authService.RegisterAsync(request);

        return authResponseDto;
    }
}