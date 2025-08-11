using MediatR;
using Restaurant.Application.Models.Restaurants;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.UseCases.Restaurant.Queries.GetRestaurantById;

internal sealed class GetRestaurantByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.Id);

        if (restaurant is null) return null!;

        var response =  new RestaurantDto(
            restaurant.Id,
            restaurant.Name,
            restaurant.Description,
            restaurant.Category,
            restaurant.HasDelivery,
            restaurant.ContactEmail,
            restaurant.Contactumber,
            restaurant.Address?.City,
            restaurant.Address?.Street,
            restaurant.Address?.PostalCode
        );
        
        return response;
    }
}