using MediatR;
using Restaurant.Application.UseCases.Restaurant.Dtos;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetRestaurantById;

public sealed record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto?>;