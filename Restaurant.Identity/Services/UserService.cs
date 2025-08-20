using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurant.Application.Common.User;
using Restaurant.Application.Contracts;

namespace Restaurant.Identity.Services;

internal sealed class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext!.User;
    
    public CurrentUser GetCurrentUser()
    {
        var id = _user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var email = _user.FindFirst(ClaimTypes.Email)!.Value;
        var userName = _user.FindFirst(ClaimTypes.Name)!.Value;
        var roles = _user.FindAll(ClaimTypes.Role!).Select(c => c.Value);

        return new CurrentUser()
        {
            Id = id,
            Email = email,
            UserName = userName,
            Roles = roles
        };
    }
}