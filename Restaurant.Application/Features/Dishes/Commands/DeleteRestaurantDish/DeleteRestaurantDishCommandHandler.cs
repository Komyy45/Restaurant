using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.Features.Dishes.Commands.DeleteRestaurantDish;
using RestaurantEntity = Domain.Entities.Restaurant;

internal sealed class DeleteRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    IRestaurantAuthorizationService restaurantAuthorizationService)  : IRequestHandler<DeleteRestaurantDishCommand>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    private readonly IGenericRepository<RestaurantEntity, int> _restaurantRepository = unitOfWork.GetRepository<RestaurantEntity, int>();
    
    public async Task Handle(DeleteRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId);

        if (restaurant is null)
            throw new NotFoundException(request.RestaurantId, nameof(RestaurantEntity));

        var isAuthorized = restaurantAuthorizationService.IsAuthorized(restaurant, ResourceOperation.Delete);

        if (!isAuthorized)
            throw new OperationForbiddenException();
        
        var getDishByIdSpecification = new GetDishByIdSpecification(request.RestaurantId);
        
        var entity = await _dishRepository.GetAsync(request.Id,d => new Dish() { Id = d.Id }, getDishByIdSpecification);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(Dish));
        
        _dishRepository.Delete(entity);

        await unitOfWork.CompleteAsync();
    }
}