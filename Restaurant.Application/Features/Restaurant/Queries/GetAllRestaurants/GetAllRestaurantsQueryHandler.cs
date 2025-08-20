using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Specifications.Restaurants;

namespace Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;

internal sealed class GetAllRestaurantsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllRestaurantsQuery, Pagination<RestaurantResponse>>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<Pagination<RestaurantResponse>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var getAllRestaurantsSpecifications = new GetAllRestaurantsSpecification(
            searchText: request.SearchText,
            pageSize: request.PageSize,
            pageNumber: request.PageNumber,
            sortBy: request.SortBy,
            sortDirection: request.SortDirection ?? false
        );
        
        var restaurants = await _restaurantRepository.GetAllAsync(getAllRestaurantsSpecifications);

        var response =  restaurants.Select(r => r.ToDto());

        int restaurantsCount = await _restaurantRepository.CountAsync();
        
        var paginationResponse = new Pagination<RestaurantResponse>(
            PageSize: request.PageSize,
            PageIndex: request.PageNumber,
            Count: restaurantsCount,
            Data: response
        );

        return paginationResponse;
    }
}