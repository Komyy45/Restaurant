using MediatR;
using Restaurant.Application.Features.Restaurant.Models.Responses;

namespace Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;

public sealed record GetAllRestaurantsQuery() : IRequest<IEnumerable<RestaurantResponse>>;