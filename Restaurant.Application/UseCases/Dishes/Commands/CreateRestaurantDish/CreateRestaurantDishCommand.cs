using MediatR;

namespace Restaurant.Application.UseCases.Dishes.Commands.CreateRestaurantDish;

public sealed record CreateRestaurantDishCommand(
    int RestaurantId,
    string Name,
    string Description,
    decimal Price,
    int? KiloCalories
    ) : IRequest<int>; 