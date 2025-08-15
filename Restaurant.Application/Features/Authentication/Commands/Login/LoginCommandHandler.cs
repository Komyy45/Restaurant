using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(IAuthService authService, 
    ILogger<LoginCommandHandler> logger) : IRequestHandler<Features.Authentication.Commands.Login.LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(Features.Authentication.Commands.Login.LoginCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing login request for user: {@Email}", request.Email);

        var response = await authService.LoginAsync(request);
        
        return response;
    }
}