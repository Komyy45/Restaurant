namespace Restaurant.Application.Features.Restaurant.Models.Responses;

public record RestaurantResponse(
    int Id,
    string Name,
    string Description,
    string Category,
    bool HasDelivery,
    string? ContactEmail,
    string? ContactNumber,
    string? City,
    string? Street,
    string? Postalcode);