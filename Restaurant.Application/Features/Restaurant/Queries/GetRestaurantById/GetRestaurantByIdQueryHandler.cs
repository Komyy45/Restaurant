using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;

internal sealed class GetRestaurantByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetRestaurantByIdQueryHandler> logger) : IRequestHandler<GetRestaurantByIdQuery, RestaurantResponse>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<RestaurantResponse> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant with Id: {@id}", request.Id);
        
        var restaurant = await _restaurantRepository.GetAsync(request.Id);

        if (restaurant is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));

        var response = restaurant.ToDto();
        
        return response;
    }
}