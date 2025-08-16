using Microsoft.AspNetCore.Identity;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;
using Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;
using Restaurant.Domain.Exceptions;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Infrastructure.Services;

internal sealed class RoleService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IRoleService
{
    public async Task AssignRoleToUser(AssignRoleToUserCommand assignRoleToUserCommand)
    {
        var user = await userManager.FindByIdAsync(assignRoleToUserCommand.UserId);
        
        if (user is null)
            throw new NotFoundException(assignRoleToUserCommand.UserId, nameof(ApplicationUser));

        var role = await roleManager.FindByNameAsync(assignRoleToUserCommand.Role);

        if (role is null) throw new NotFoundException(assignRoleToUserCommand.Role, nameof(IdentityRole));
        
        var result = await userManager.AddToRoleAsync(user, assignRoleToUserCommand.Role);

        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code.ToString(), e => e.Description);
            throw new ValidationException(errors);
        }
    }

    public  async Task RemoveRoleFromUser(RemoveRoleFromUserCommand removeRoleFromUserCommand)
    {
        var user = await userManager.FindByIdAsync(removeRoleFromUserCommand.UserId);
        
        if (user is null)
            throw new NotFoundException(removeRoleFromUserCommand.UserId, nameof(ApplicationUser));

        var role = await roleManager.FindByNameAsync(removeRoleFromUserCommand.Role);

        if (role is null) throw new NotFoundException(removeRoleFromUserCommand.Role, nameof(IdentityRole));
        
        var result = await userManager.RemoveFromRoleAsync(user, removeRoleFromUserCommand.Role);

        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code.ToString(), e => e.Description);
            throw new ValidationException(errors);
        }
    }
}