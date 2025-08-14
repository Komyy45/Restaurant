namespace Restaurant.Application.UseCases.Authentication.Dtos;

public sealed record AuthResponseDto(string AccessToken, string RefreshToken);