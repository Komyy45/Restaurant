using MediatR;

namespace Restaurant.Application.UseCases.Restaurant.Commands.DeleteRestaurant;

public sealed record DeleteRestaurantCommand(int Id) : IRequest;