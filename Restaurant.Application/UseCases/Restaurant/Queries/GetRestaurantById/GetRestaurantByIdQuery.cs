using MediatR;
using Restaurant.Application.Models.Restaurants;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetRestaurantById;

public sealed record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto?>;