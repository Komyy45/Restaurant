namespace Restaurant.Application.UseCases.Dishes.Dtos;

public sealed record DishDto(
    string Name,
    string Description,
    decimal Price,
    int? KiloCalories,
    int RestaurantId
    );