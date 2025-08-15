using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Authentication.Commands.UpdateAccount;

internal sealed class UpdateAccountCommandHandler(
    IAuthService authService) 
    : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        await authService.UpdateAccount(request);
    }
}
