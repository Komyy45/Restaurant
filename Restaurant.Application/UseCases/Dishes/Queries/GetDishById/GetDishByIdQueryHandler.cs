using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Application.UseCases.Dishes.Dtos;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.UseCases.Dishes.Queries.GetDishById;

internal sealed class GetDishByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetDishByIdQueryHandler> logger) : IRequestHandler<GetDishByIdQuery, DishDto>
{
    private readonly IGenericRepository<Dish, int> _dishesRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task<DishDto> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish with Id: {@id}", request.Id);
        
        var getDishByIdSpecification = new GetDishByIdSpecification(request.RestaurantId);

        var dish = await _dishesRepository.GetAsync(request.Id, getDishByIdSpecification);
        
        return dish?.ToDto()!;
    }
}