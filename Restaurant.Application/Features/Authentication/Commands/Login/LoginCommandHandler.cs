using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(Features.Authentication.Commands.Login.LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request);
        
        return response;
    }
}