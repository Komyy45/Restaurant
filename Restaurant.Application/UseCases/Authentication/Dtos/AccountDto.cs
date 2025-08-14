namespace Restaurant.Application.UseCases.Authentication.Dtos;

public sealed record AccountDto(
    string Id,
    string Email,
    string UserName,
    DateOnly DateOfBirth,
    string FullName,
    string PhoneNumber
    );