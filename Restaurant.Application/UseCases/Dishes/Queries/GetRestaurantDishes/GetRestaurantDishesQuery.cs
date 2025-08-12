using MediatR;
using Restaurant.Application.UseCases.Dishes.Dtos;

namespace Restaurant.Application.UseCases.Dishes.Queries.GetRestaurantDishes;

public sealed record GetRestaurantDishesQuery(
    int RestaurantId
        ) : IRequest<IEnumerable<DishDto>>;