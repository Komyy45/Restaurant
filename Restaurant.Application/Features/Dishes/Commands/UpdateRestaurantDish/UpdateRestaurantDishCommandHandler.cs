using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using RestaurantEntity = Domain.Entities.Restaurant;

internal sealed class UpdateRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    IRestaurantAuthorizationService restaurantAuthorizationService)  : IRequestHandler<UpdateRestaurantDishCommand>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    private readonly IGenericRepository<RestaurantEntity, int> _restaurantRepository = unitOfWork.GetRepository<RestaurantEntity, int>();
    
    public async Task Handle(UpdateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId);

        if (restaurant is null)
            throw new NotFoundException(request.RestaurantId, nameof(RestaurantEntity));

        var isAuthorized = restaurantAuthorizationService.IsAuthorized(restaurant, ResourceOperation.Update);

        if (!isAuthorized)
            throw new OperationForbiddenException();
        
        
        var dish = await  _dishRepository.GetAsync(request.Id);

        if (dish is null || dish.RestaurantId != request.RestaurantId)
            throw new NotFoundException(request.Id, nameof(Dish));
        
        dish = request.ToEntity(dish);
        
        _dishRepository.Update(dish);

        await unitOfWork.CompleteAsync();
    }
}