using MediatR;

namespace Restaurant.Application.Features.Authentication.Commands.RevokeToken;

public sealed record RevokeTokenCommand(string RefreshToken) : IRequest;