using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using RestaurantEntity = Domain.Entities.Restaurant;
internal sealed class CreateRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    IUserService userService) : IRequestHandler<CreateRestaurantDishCommand, int>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    private readonly IGenericRepository<RestaurantEntity, int> _restaurantRepository = unitOfWork.GetRepository<RestaurantEntity, int>();
    
    public async Task<int> Handle(CreateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId);

        if (restaurant is null)
            throw new NotFoundException(request.RestaurantId, nameof(RestaurantEntity));

        var isAuthorized = restaurant.OwnerId == userService.GetCurrentUser().Id;

        if (!isAuthorized)
            throw new OperationForbiddenException();
        
        var dish = request.ToEntity();
        
        await _dishRepository.CreateAsync(dish);

        await unitOfWork.CompleteAsync();

        return dish.Id;
    }
}