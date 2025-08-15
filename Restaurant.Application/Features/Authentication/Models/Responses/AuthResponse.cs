namespace Restaurant.Application.Features.Authentication.Models.Responses;

public sealed record AuthResponse(string AccessToken, string RefreshToken);