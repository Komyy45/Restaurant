using MediatR;
using Restaurant.Application.Contracts;

namespace Restaurant.Application.Features.Users.Commands.AssignRoleToUser;

internal sealed class AssignRoleToUserCommandHandler(IRoleService roleService) : IRequestHandler<AssignRoleToUserCommand>
{
    public async Task Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        await roleService.AssignRoleToUser(request);
    }
}