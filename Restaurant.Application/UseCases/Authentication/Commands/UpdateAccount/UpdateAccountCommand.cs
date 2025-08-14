using MediatR;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Commands.UpdateAccount;

public sealed record UpdateAccountCommand(
    string Id,
    string UserName,
    DateOnly DateOfBirth,
    string FullName,
    string PhoneNumber
    ) : IRequest;