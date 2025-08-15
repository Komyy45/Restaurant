using MediatR;
using Restaurant.Application.Features.Restaurant.Models.Responses;

namespace Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;

public sealed record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantResponse>;