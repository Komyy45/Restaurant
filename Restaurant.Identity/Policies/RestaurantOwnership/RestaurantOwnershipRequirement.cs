using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Infrastructure.Policies.RestaurantOwnership;

internal sealed class RestaurantOwnershipRequirement(uint minimumNumberOfOwnedRestaurants) : IAuthorizationRequirement
{
    public uint MinimumNumberOfOwnedRestaurants => minimumNumberOfOwnedRestaurants;
}