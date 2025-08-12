using MediatR;
using Restaurant.Application.Mapping;
using Restaurant.Application.Models.Restaurants;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetAllRestaurants;

internal sealed class GetAllRestaurantsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetAllAsync();

        var response =  restaurants.Select(r => r.ToDto());
        
        return response;
    }
}