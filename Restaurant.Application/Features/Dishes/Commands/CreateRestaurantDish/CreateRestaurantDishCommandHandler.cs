using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;

internal sealed class CreateRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateRestaurantDishCommandHandler> logger) : IRequestHandler<CreateRestaurantDishCommand, int>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task<int> Handle(CreateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new Dish {@dish}.", request);

        var dish = request.ToEntity();
        
        await _dishRepository.CreateAsync(dish);

        await unitOfWork.CompleteAsync();

        return dish.Id;
    }
}