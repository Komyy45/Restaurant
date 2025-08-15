using MediatR;

namespace Restaurant.Application.Features.Authentication.Commands.UpdateAccount;

public sealed record UpdateAccountCommand(
    string Id,
    string UserName,
    DateOnly DateOfBirth,
    string FullName,
    string PhoneNumber
    ) : IRequest;