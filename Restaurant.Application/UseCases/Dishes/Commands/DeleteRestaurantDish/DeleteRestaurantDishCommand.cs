using MediatR;

namespace Restaurant.Application.UseCases.Dishes.Commands.DeleteRestaurantDish;

public sealed record DeleteRestaurantDishCommand(
    int Id,
    int RestaurantId
    ) : IRequest;