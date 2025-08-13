using Mapster;
using Restaurant.Application.UseCases.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.UseCases.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.UseCases.Dishes.Dtos;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mapping;

internal static class DishesMappingProfile
{
    internal static DishDto ToDto(this Dish dish) => dish.Adapt<DishDto>();
    
    internal static Dish ToEntity(this DishDto dishDto) => dishDto.Adapt<Dish>();

    internal static Dish ToEntity(this DishDto dishDto, Dish entity) => dishDto.Adapt(entity);
    
    internal static Dish  ToEntity(this CreateRestaurantDishCommand dishDto) => dishDto.Adapt<Dish>();
    
    internal static Dish ToEntity(this UpdateRestaurantDishCommand dishDto) => dishDto.Adapt<Dish>();
    
    internal static Dish ToEntity(this UpdateRestaurantDishCommand dishDto, Dish entity) => dishDto.Adapt(entity);
}