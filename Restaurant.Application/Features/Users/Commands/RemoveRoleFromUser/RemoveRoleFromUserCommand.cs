using MediatR;

namespace Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;

public sealed record RemoveRoleFromUserCommand(string UserId, string Role) : IRequest;