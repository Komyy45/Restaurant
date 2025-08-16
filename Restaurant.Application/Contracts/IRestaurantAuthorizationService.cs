using Restaurant.Application.Common.Enums;

namespace Restaurant.Application.Contracts;
using RestaurantEntity = Domain.Entities.Restaurant;


public interface IRestaurantAuthorizationService
{
    public bool IsAuthorized(RestaurantEntity restaurant, ResourceOperation operation);
}