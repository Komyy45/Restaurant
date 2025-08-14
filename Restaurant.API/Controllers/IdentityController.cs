using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.UseCases.Authentication.Commands.LoginCommand;
using Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;
using Restaurant.Application.UseCases.Authentication.Commands.UpdateAccount;
using Restaurant.Application.UseCases.Authentication.Dtos;
using Restaurant.Application.UseCases.Authentication.Queries.GetCurrentUserAccount;

namespace Restaurant.API.Controllers;

public sealed class IdentityController(IMediator mediator) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterCommand request)
    {
        return await mediator.Send(request);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand request)
    {
        return await mediator.Send(request);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("info")]
    public async Task<ActionResult<AccountDto>> GetCurrentUserProfile()
    {
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var request = new GetCurrentUserAccountQuery(id);
        return await mediator.Send(request);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPatch("info")]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand request)
    {
        await mediator.Send(request);
        return NoContent();
    }
}