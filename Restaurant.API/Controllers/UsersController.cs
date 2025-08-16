using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;
using Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;
using Restaurant.Domain.Common;

namespace Restaurant.API.Controllers;

[Authorize(Roles = RoleTypes.Admin)]
public sealed class UsersController(IMediator mediator) : BaseApiController
{
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand assignRoleToUserCommand)
    {
        await mediator.Send(assignRoleToUserCommand);
        return NoContent();
    }
    
    [HttpDelete("remove-role")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleFromUserCommand removeRoleFromUserCommand)
    {
        await mediator.Send(removeRoleFromUserCommand);
        return NoContent();
    }
}