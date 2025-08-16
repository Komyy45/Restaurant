using MediatR;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;

internal sealed class RemoveRoleFromUserCommandHandler(IRoleService roleService) : IRequestHandler<RemoveRoleFromUserCommand>
{
    public async Task Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        await roleService.RemoveRoleFromUser(request);
    }
}