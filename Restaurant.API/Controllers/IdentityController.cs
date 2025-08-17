using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.Features.Authentication.Commands.Login;
using Restaurant.Application.Features.Authentication.Commands.RefreshToken;
using Restaurant.Application.Features.Authentication.Commands.Register;
using Restaurant.Application.Features.Authentication.Commands.RevokeToken;
using Restaurant.Application.Features.Authentication.Commands.UpdateAccount;
using Restaurant.Application.Features.Authentication.Models.Responses;
using Restaurant.Application.Features.Authentication.Queries.GetCurrentUserAccount;

namespace Restaurant.API.Controllers;

public sealed class IdentityController(IMediator mediator) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterCommand request)
    {
        return await mediator.Send(request);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginCommand request)
    {
        return await mediator.Send(request);
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<AccountResponse>> GetCurrentUserProfile()
    {
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var request = new GetCurrentUserAccountQuery(id);
        return await mediator.Send(request);
    }

    [HttpPatch("info")]
    [Authorize]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand request)
    {
        await mediator.Send(request);
        return NoContent();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromQuery] RefreshTokenCommand request)
    {
        return await mediator.Send(request);
    }
    
    [HttpDelete("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromQuery] RevokeTokenCommand request)
    {
        await mediator.Send(request);
        return NoContent();
    }
}