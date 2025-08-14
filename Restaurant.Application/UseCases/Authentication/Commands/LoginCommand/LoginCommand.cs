using MediatR;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Commands.LoginCommand;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;