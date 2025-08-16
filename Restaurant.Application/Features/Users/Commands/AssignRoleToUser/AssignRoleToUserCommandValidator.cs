using FluentValidation;
using Restaurant.Domain.Common;

namespace Restaurant.Application.Features.Users.Commands.AssignRoleToUser;

internal sealed class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    private readonly IReadOnlyList<string> _validRoles = [RoleTypes.Admin, RoleTypes.Owner, RoleTypes.Customer];
    
    public AssignRoleToUserCommandValidator()
    {
        RuleFor(r => r.Role)
            .Must(role => _validRoles.Contains(role));
    }
}