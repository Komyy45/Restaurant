using Mapster;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurant.Models.Responses;

namespace Restaurant.Application.Mapping;

using RestaurantEntity = Domain.Entities.Restaurant;


internal static class RestaurantsMappingProfile
{
    internal static RestaurantResponse ToDto(this RestaurantEntity restaurant) => restaurant.Adapt<RestaurantResponse>();
    internal static RestaurantEntity ToEntity(this RestaurantResponse restaurantResponse) => restaurantResponse.Adapt<RestaurantEntity>();
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand) => updateRestaurantCommand.Adapt<RestaurantEntity>();
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand, RestaurantEntity entity) => updateRestaurantCommand.Adapt(entity);

    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand)
    {
         var entity = createRestaurantCommand.Adapt<RestaurantEntity>();
         entity.Address = new();
         entity.Address.City = createRestaurantCommand.City;
         entity.Address.Street = createRestaurantCommand.Street;
         entity.Address.PostalCode = createRestaurantCommand.PostalCode;
         entity.Contactumber = createRestaurantCommand.ContactNumber;
         return entity;
    }
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand, RestaurantEntity entity) => createRestaurantCommand.Adapt(entity);
}