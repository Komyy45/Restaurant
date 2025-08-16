using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Contracts;

namespace Restaurant.Infrastructure.Policies.RestaurantOwnership;
using RestaurantEntity = Domain.Entities.Restaurant;
internal sealed class RestaurantOwnershipRequirementHandler(IUnitOfWork unitOfWork,
    ILogger<RestaurantOwnershipRequirementHandler> logger) : AuthorizationHandler<RestaurantOwnershipRequirement>
{
    private readonly IGenericRepository<RestaurantEntity, int> _restaurantRepository =
        unitOfWork.GetRepository<RestaurantEntity, int>();
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RestaurantOwnershipRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            logger.LogWarning("Authorization failed: user is not authenticated.");
            context.Fail();
            return;
        }

        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            logger.LogWarning("Authorization failed: unable to retrieve current user ID.");
            context.Fail();
            return;
        }

        var ownedRestaurantsCount = await _restaurantRepository.CountAsync(r => r.OwnerId == userId);

        if (ownedRestaurantsCount < requirement.MinimumNumberOfOwnedRestaurants)
        {
            logger.LogInformation(
                "Authorization failed: User {UserId} owns {OwnedCount} restaurants, requires at least {RequiredCount}.",
                userId, ownedRestaurantsCount, requirement.MinimumNumberOfOwnedRestaurants
            );
            context.Fail();
            return;
        }

        logger.LogInformation("Authorization succeeded for user {UserId}.", userId);
        context.Succeed(requirement);
    }

}