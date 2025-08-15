using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;

internal sealed class GetAllRestaurantsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantResponse>>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<IEnumerable<RestaurantResponse>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetAllAsync();

        var response =  restaurants.Select(r => r.ToDto());
        
        return response;
    }
}