namespace Restaurant.Application.Features.Authentication.Models.Responses;

public sealed record AccountResponse(
    string Id,
    string Email,
    string UserName,
    DateOnly DateOfBirth,
    string FullName,
    string PhoneNumber
    );