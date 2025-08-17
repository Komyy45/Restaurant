using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mapping;

internal static class DishesMappingProfile
{
    internal static DishResponse ToDto(this Dish dish)
    {
        var dto = new DishResponse(
            Id: dish.Id,
            Name: dish.Name,
            Description: dish.Description,
            Price: dish.Price,
            KiloCalories: dish.KiloCalories,
            RestaurantId: dish.RestaurantId
        );
        return dto;
    }
    internal static Dish ToEntity(this CreateRestaurantDishCommand dishDto)
    {
        var entity = new Dish()
        {
            Name = dishDto.Name,
            Description = dishDto.Description,
            Price = dishDto.Price,
            KiloCalories = dishDto.KiloCalories,
            RestaurantId = dishDto.RestaurantId
        };
        return entity;
    }
    internal static Dish ToEntity(this UpdateRestaurantDishCommand dishDto, Dish existingEntity)
    {
        var entity = new Dish()
        {
            Id = existingEntity.Id,
            Name = dishDto.Name,
            Description = dishDto.Description,
            Price = dishDto.Price,
            KiloCalories = dishDto.KiloCalories,
            RestaurantId = dishDto.RestaurantId
        };
        return entity;
    }
}