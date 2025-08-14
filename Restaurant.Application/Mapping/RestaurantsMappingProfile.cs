using Mapster;
using Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Dtos;

namespace Restaurant.Application.Mapping;

using RestaurantEntity = Domain.Entities.Restaurant;


internal static class RestaurantsMappingProfile
{
    internal static RestaurantDto ToDto(this RestaurantEntity restaurant) => restaurant.Adapt<RestaurantDto>();
    internal static RestaurantEntity ToEntity(this RestaurantDto restaurantDto) => restaurantDto.Adapt<RestaurantEntity>();
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand) => updateRestaurantCommand.Adapt<RestaurantEntity>();
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand, RestaurantEntity entity) => updateRestaurantCommand.Adapt(entity);
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand) => createRestaurantCommand.Adapt<RestaurantEntity>();
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand, RestaurantEntity entity) => createRestaurantCommand.Adapt(entity);
}