namespace Restaurant.Application.Features.Dishes.Models.Responses;

public sealed record DishResponse(
    string Name,
    string Description,
    decimal Price,
    int? KiloCalories,
    int RestaurantId
    );