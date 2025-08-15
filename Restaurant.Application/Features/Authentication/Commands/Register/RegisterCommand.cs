using MediatR;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(string UserName, string Email, string Password, string FullName, DateOnly DateOfBirth) : IRequest<AuthResponse>;