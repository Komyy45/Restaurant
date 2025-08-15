using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Authentication.Commands.UpdateAccount;

internal sealed class UpdateAccountCommandHandler(
    IAuthService authService,
    ILogger<UpdateAccountCommandHandler> logger) 
    : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting account update for AccountId: {Id}", request.Id);

        await authService.UpdateAccount(request);
    }
}
