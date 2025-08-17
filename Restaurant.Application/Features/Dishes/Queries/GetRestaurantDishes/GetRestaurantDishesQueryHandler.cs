using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;


internal sealed class GetRestaurantDishesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRestaurantDishesQuery, Pagination<DishResponse>>
{
    private readonly IGenericRepository<Dish, int> _dishesRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task<Pagination<DishResponse>> Handle(GetRestaurantDishesQuery request, CancellationToken cancellationToken)
    {
        var getRestaurantDishesSpecification = new GetRestaurantDishesSpecification(
            pageSize: request.PageSize,
            pageIndex: request.PageNumber,
            searchText: request.SearchText,
            sortBy: request.SortBy,
            sortDirection: request.SortDirection ?? false,
            restaurantId: request.RestaurantId
            );
        
        var dishes = await _dishesRepository.GetAllAsync(getRestaurantDishesSpecification);

        if (dishes is null) 
            throw new NotFoundException(request.RestaurantId, nameof(Restaurant));

        var response = dishes.Select(d => d.ToDto());

        var restaurantDishesCount = await _dishesRepository.CountAsync(d => d.RestaurantId == request.RestaurantId);
        
        var paginationResponse = new Pagination<DishResponse>(
            PageIndex: request.PageNumber,
            PageSize: request.PageSize,
            Count: restaurantDishesCount,
            Data: response
            );
        
        return paginationResponse;
    }
}