using MediatR;

namespace Restaurant.Application.Features.Users.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(string UserId, string Role) : IRequest;