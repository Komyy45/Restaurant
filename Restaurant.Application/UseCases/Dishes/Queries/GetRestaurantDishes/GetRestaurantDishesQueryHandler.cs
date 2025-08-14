using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Application.UseCases.Dishes.Dtos;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.UseCases.Dishes.Queries.GetRestaurantDishes;
using RestaurantEntity = Domain.Entities.Restaurant;


internal sealed class GetRestaurantDishesQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetRestaurantDishesQueryHandler> logger) : IRequestHandler<GetRestaurantDishesQuery, IEnumerable<DishDto>>
{
    private readonly IGenericRepository<RestaurantEntity, int> _restaurantRepository = unitOfWork.GetRepository<RestaurantEntity, int>();
    
    public async Task<IEnumerable<DishDto>> Handle(GetRestaurantDishesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Dishes for Restaurant with Id: {@restaurantId}", request.RestaurantId);
        
        var getRestaurantDishesSpecification = new GetRestaurantDishesSpecification();
        
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId, 
            r=> new { r.Dishes }, 
            getRestaurantDishesSpecification);

        if (restaurant is null) return null!;

        var response = restaurant.Dishes.Select(d => d.ToDto());

        return response;
    }
}