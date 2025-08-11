using MediatR;
using Restaurant.Application.Models.Restaurants;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetAllRestaurants;

public sealed record GetAllRestaurantsQuery() : IRequest<IEnumerable<RestaurantDto>>;