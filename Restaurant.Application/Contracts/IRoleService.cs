using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;
using Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;

namespace Restaurant.Application.Contracts;

public interface IRoleService
{
    public Task AssignRoleToUser(AssignRoleToUserCommand assignRoleToUserCommand);

    public Task RemoveRoleFromUser(RemoveRoleFromUserCommand removeRoleFromUserCommand);
}