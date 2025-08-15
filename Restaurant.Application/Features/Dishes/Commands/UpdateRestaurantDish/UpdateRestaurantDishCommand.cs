using MediatR;

namespace Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;

public sealed record UpdateRestaurantDishCommand(
    int Id, 
    int RestaurantId,     
    string Name,
    string Description,
    decimal Price,
    int? KiloCalories
    ) : IRequest;