namespace Restaurant.Application.Models.Restaurants;

public record RestaurantDto(
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