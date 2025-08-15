using Mapster;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mapping;

internal static class DishesMappingProfile
{
    internal static DishResponse ToDto(this Dish dish) => dish.Adapt<DishResponse>();
    internal static Dish ToEntity(this DishResponse dishResponse) => dishResponse.Adapt<Dish>();
    internal static Dish ToEntity(this DishResponse dishResponse, Dish entity) => dishResponse.Adapt(entity);
    internal static Dish  ToEntity(this CreateRestaurantDishCommand dishDto) => dishDto.Adapt<Dish>();
    internal static Dish ToEntity(this UpdateRestaurantDishCommand dishDto) => dishDto.Adapt<Dish>();
    internal static Dish ToEntity(this UpdateRestaurantDishCommand dishDto, Dish entity) => dishDto.Adapt(entity);
}