using MediatR;

namespace Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;

public sealed record UpdateRestaurantCommand(
    int Id,
    string Name,
    string Description,
    bool HasDelivery
    ) : IRequest;