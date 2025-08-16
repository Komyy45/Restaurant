using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandHandler(IUnitOfWork unitOfWork, 
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IUserService userService) : IRequestHandler<CreateRestaurantCommand, int>
{
    private IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository = unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = request.ToEntity();

        var isAuthorized = restaurantAuthorizationService.IsAuthorized(restaurant, ResourceOperation.Create);

        if (!isAuthorized) throw new OperationForbiddenException();

        restaurant.OwnerId = userService.GetCurrentUser().Id;
        
        await _restaurantRepository.CreateAsync(restaurant);
        
        await unitOfWork.CompleteAsync();
        
        return restaurant.Id;
    }
}