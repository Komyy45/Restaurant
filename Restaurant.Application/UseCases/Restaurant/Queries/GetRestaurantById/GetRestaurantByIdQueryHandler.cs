using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Application.UseCases.Restaurant.Dtos;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetRestaurantById;

internal sealed class GetRestaurantByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetRestaurantByIdQueryHandler> logger) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant with Id: {@id}", request.Id);
        
        var restaurant = await _restaurantRepository.GetAsync(request.Id);

        if (restaurant is null) return null!;

        var response = restaurant.ToDto();
        
        return response;
    }
}