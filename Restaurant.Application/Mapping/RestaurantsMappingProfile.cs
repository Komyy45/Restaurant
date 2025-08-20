using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mapping;

using RestaurantEntity = Domain.Entities.Restaurant;


internal static class RestaurantsMappingProfile
{
    internal static RestaurantResponse ToDto(this RestaurantEntity restaurant)
    {
        var dto = new RestaurantResponse(
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            HasDelivery: restaurant.HasDelivery,
            Category: restaurant.Category,
            ContactEmail: restaurant.ContactEmail,
            ContactNumber: restaurant.ContactNumber,
            City: restaurant.Address?.City,
            Street: restaurant.Address?.Street,
            Postalcode: restaurant.Address?.PostalCode
        );
        
        var restaurantAddress = restaurant.Address;
        if (restaurantAddress is not null)
        {
           
            dto = dto with
            {
                City = restaurantAddress.City,
                Street = restaurantAddress.Street,
                Postalcode = restaurantAddress.PostalCode,
            };
        }
        
        return dto;
    }
    internal static RestaurantEntity ToEntity(this UpdateRestaurantCommand updateRestaurantCommand, RestaurantEntity existingEntity)
    {
        var entity = new RestaurantEntity()
        {
            Id = existingEntity.Id,
            Name = updateRestaurantCommand.Name,
            Description = updateRestaurantCommand.Description,
            Category = existingEntity.Category,
            ContactNumber = existingEntity.ContactNumber,
            ContactEmail = existingEntity.ContactEmail,
            HasDelivery = updateRestaurantCommand.HasDelivery,
            OwnerId = existingEntity.OwnerId
        };
        
        var existingEntityAddress = existingEntity.Address;
        if (existingEntityAddress is not null)
        {
            entity.Address = new()
            {
                City = existingEntityAddress.City,
                Street = existingEntityAddress.Street,
                PostalCode = existingEntityAddress.PostalCode,
            };
        }
        
        return entity;
    }
    internal static RestaurantEntity ToEntity(this CreateRestaurantCommand createRestaurantCommand, string ownerId)
    {
         var entity = new RestaurantEntity()
         {
             Name = createRestaurantCommand.Name,
             Description = createRestaurantCommand.Description,
             Category = createRestaurantCommand.Category,
             ContactNumber = createRestaurantCommand.ContactNumber,
             ContactEmail = createRestaurantCommand.ContactEmail,
             Address = new()
             {
                 City = createRestaurantCommand.City,
                 Street = createRestaurantCommand.Street,
                 PostalCode = createRestaurantCommand.PostalCode
             },
             HasDelivery = createRestaurantCommand.HasDelivery,
             OwnerId = ownerId
         };

         return entity;
    }
}