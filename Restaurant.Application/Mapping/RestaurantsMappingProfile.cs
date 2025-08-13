using Mapster;
using Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Dtos;

namespace Restaurant.Application.Mapping;

using RestaurantEntity = Domain.Entities.Restaurant;


internal static class RestaurantsMappingProfile
{
    internal static RestaurantDto ToDto(this RestaurantEntity restaurant)
    {
        var restaurantDto = restaurant.Adapt<RestaurantDto>();
        return restaurantDto;
    }

    internal static RestaurantEntity ToEntity(this RestaurantDto restaurantDto)
    {
        var restaurant = restaurantDto.Adapt<RestaurantEntity>();
        return restaurant;
    }

    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand)
    {
        var restaurant = updateRestaurantCommand.Adapt<RestaurantEntity>();
        return restaurant;
    }
    
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand, RestaurantEntity entity)
    {
        var restaurant = updateRestaurantCommand.Adapt(entity);
        return restaurant;
    }
    
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand)
    {
        var restaurant = createRestaurantCommand.Adapt<RestaurantEntity>();
        return restaurant;
    }
    
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand, RestaurantEntity entity)
    {
        var restaurant = createRestaurantCommand.Adapt(entity);
        return restaurant;
    }
}