using MediatR;
using Restaurant.Application.Features.Dishes.Models.Responses;

namespace Restaurant.Application.Features.Dishes.Queries.GetDishById;

public sealed record GetDishByIdQuery(int RestaurantId, int Id) : IRequest<DishResponse>;