using MediatR;
using Restaurant.Application.Features.Dishes.Models.Responses;

namespace Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;

public sealed record GetRestaurantDishesQuery(
    int RestaurantId
        ) : IRequest<IEnumerable<DishResponse>>;