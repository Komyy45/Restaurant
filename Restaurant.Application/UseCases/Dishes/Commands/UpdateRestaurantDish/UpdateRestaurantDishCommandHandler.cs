using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Application.UseCases.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.UseCases.Dishes.Commands.UpdateRestaurantDish;

internal sealed class UpdateRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateRestaurantDishCommandHandler> logger)  : IRequestHandler<UpdateRestaurantDishCommand>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task Handle(UpdateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a Dish.");
        
        var dish = await  _dishRepository.GetAsync(request.Id);

        if (dish is null) return;
        
        dish = request.ToEntity(dish);
        
        _dishRepository.Update(dish);

        await unitOfWork.CompleteAsync();
    }
}