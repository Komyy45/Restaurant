using MediatR;
using Restaurant.Application.UseCases.Dishes.Dtos;

namespace Restaurant.Application.UseCases.Dishes.Queries.GetDishById;

public sealed record GetDishByIdQuery(int RestaurantId, int Id) : IRequest<DishDto>;