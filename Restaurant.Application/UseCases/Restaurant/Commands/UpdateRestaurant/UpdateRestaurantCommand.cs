using MediatR;

namespace Restaurant.Application.UseCases.Restaurant.Commands.UpdateRestaurant;

public sealed record UpdateRestaurantCommand(
    int Id,
    string Name,
    string Description,
    bool HasDelivery
    ) : IRequest<bool>;