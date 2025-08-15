using MediatR;

namespace Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;

public sealed record DeleteRestaurantCommand(int Id) : IRequest;