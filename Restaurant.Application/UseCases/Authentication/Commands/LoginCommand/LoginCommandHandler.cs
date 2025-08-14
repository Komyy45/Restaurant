using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Commands.LoginCommand;

internal sealed class LoginCommandHandler(IAuthService authService, 
    ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing login request for user: {@Email}", request.Email);

        var response = await authService.LoginAsync(request);
        
        return response;
    }
}