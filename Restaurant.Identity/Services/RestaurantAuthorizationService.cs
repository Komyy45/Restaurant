using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Domain.Common;

namespace Restaurant.Identity.Services;
using RestaurantEntity = Domain.Entities.Restaurant;

internal sealed class RestaurantAuthorizationService(
    ILogger<RestaurantAuthorizationService> logger,
    IUserService userService) : IRestaurantAuthorizationService
{
    public bool IsAuthorized(RestaurantEntity restaurant, ResourceOperation operation)
    {
        var user = userService.GetCurrentUser();

        logger.LogInformation(
            "Authorization check started. UserId: {UserId}, Roles: {Roles}, Operation: {Operation}, RestaurantId: {RestaurantId}",
            user.Id,
            string.Join(",", user.Roles),
            operation,
            restaurant.Id);

        bool isAuthorized = false;

        if (operation == ResourceOperation.Delete && user.IsInRole(RoleTypes.Admin))
        {
            logger.LogInformation("User is Admin and attempting Delete. Authorization granted.");
            isAuthorized = true;
        }
        else if ((operation == ResourceOperation.Update || operation == ResourceOperation.Delete)
                 && user.IsInRole(RoleTypes.Owner) && restaurant.OwnerId == user.Id)
        {
            logger.LogInformation("User is Owner and attempting Update/Delete. Authorization granted.");
            isAuthorized = true;
        }
        else if (operation == ResourceOperation.Create && user.IsInRole(RoleTypes.Owner))
        {
            logger.LogInformation("User is Owner and attempting Create. Authorization granted.");
            isAuthorized = true;
        }
        else if (operation == ResourceOperation.Read)
        {
            logger.LogInformation("Operation is Read. Authorization granted to all users.");
            isAuthorized = true;
        }

        logger.LogInformation(
            "Authorization result: {Result}. UserId: {UserId}, Operation: {Operation}, RestaurantId: {RestaurantId}",
            isAuthorized ? "Granted" : "Denied",
            user.Id,
            operation,
            restaurant.Id);

        return isAuthorized;
    }
}