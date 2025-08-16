using Mapster;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mapping;
using RestaurantEntity = Domain.Entities.Restaurant;

internal sealed class MapsterConfig
{
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateRestaurantCommand, RestaurantEntity>
                .NewConfig()
                .Map(dest => dest.Address, src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode
                })
                .Map(dest => dest.Contactumber, src => src.ContactNumber);

            TypeAdapterConfig<RestaurantEntity, RestaurantResponse>
                .NewConfig();
        }
}