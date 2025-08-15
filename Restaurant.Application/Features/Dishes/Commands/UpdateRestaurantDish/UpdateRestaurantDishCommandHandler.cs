using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;

internal sealed class UpdateRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateRestaurantDishCommandHandler> logger)  : IRequestHandler<UpdateRestaurantDishCommand>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task Handle(UpdateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a Dish {@dish}.", request);
        
        var dish = await  _dishRepository.GetAsync(request.Id);

        if (dish is null || dish.RestaurantId != request.RestaurantId)
            throw new NotFoundException(request.Id, nameof(Dish));
        
        dish = request.ToEntity(dish);
        
        _dishRepository.Update(dish);

        await unitOfWork.CompleteAsync();
    }
}