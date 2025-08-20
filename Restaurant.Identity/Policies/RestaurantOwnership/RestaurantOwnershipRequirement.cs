using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Identity.Policies.RestaurantOwnership;

internal sealed class RestaurantOwnershipRequirement(uint minimumNumberOfOwnedRestaurants) : IAuthorizationRequirement
{
    public uint MinimumNumberOfOwnedRestaurants => minimumNumberOfOwnedRestaurants;
}