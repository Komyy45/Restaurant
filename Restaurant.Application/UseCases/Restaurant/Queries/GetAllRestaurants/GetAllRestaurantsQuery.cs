using MediatR;
using Restaurant.Application.UseCases.Restaurant.Dtos;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetAllRestaurants;

public sealed record GetAllRestaurantsQuery() : IRequest<IEnumerable<RestaurantDto>>;