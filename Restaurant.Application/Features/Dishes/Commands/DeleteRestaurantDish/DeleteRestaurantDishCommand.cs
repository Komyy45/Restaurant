using MediatR;

namespace Restaurant.Application.Features.Dishes.Commands.DeleteRestaurantDish;

public sealed record DeleteRestaurantDishCommand(
    int Id,
    int RestaurantId
    ) : IRequest;