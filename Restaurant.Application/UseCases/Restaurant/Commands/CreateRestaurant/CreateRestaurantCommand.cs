using MediatR;

namespace Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;

public sealed record CreateRestaurantCommand(
    string Name,
    string Description,
    string Category,
    bool HasDelivery,
    string? ContactEmail,
    string? ContactNumber,
    string? City,
    string? Street,
    string? PostalCode
    ) : IRequest<int>;