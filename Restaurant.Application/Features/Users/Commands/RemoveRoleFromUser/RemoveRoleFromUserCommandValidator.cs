using FluentValidation;
using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;
using Restaurant.Domain.Common;

namespace Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;

internal sealed class RemoveRoleFromUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    private readonly IReadOnlyList<string> _validRoles = [RoleTypes.Admin, RoleTypes.Owner, RoleTypes.Customer];
    
    public RemoveRoleFromUserCommandValidator()
    {
        RuleFor(r => r.Role)
            .Must(role => _validRoles.Contains(role));
    }
}