using MediatR;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;

public sealed record RegisterCommand(string UserName, string Email, string Password, string FullName, DateOnly DateOfBirth) : IRequest<AuthResponseDto>;