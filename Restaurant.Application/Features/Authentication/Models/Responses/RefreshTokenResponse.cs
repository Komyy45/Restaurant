namespace Restaurant.Application.Features.Authentication.Models.Responses;

public sealed record RefreshTokenResponse(string AccessToken, string RefreshToken);