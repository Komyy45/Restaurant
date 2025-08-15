using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.Features.Dishes.Queries.GetDishById;

internal sealed class GetDishByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetDishByIdQueryHandler> logger) : IRequestHandler<GetDishByIdQuery, DishResponse>
{
    private readonly IGenericRepository<Dish, int> _dishesRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task<DishResponse> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish with Id: {@id}", request.Id);
        
        var getDishByIdSpecification = new GetDishByIdSpecification(request.RestaurantId);

        var dish = await _dishesRepository.GetAsync(request.Id, getDishByIdSpecification);

        if (dish is null)
            throw new NotFoundException(request.Id, nameof(Dish));
        
        return dish?.ToDto()!;
    }
}