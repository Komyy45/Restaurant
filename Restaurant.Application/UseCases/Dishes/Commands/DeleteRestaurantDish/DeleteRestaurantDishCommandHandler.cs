using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.UseCases.Dishes.Commands.DeleteRestaurantDish;

internal sealed class DeleteRestaurantDishCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeleteRestaurantDishCommandHandler> logger)  : IRequestHandler<DeleteRestaurantDishCommand>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task Handle(DeleteRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting a Dish.");
        
        var getDishByIdSpecification = new GetDishByIdSpecification(request.RestaurantId);
        
        var entity = await _dishRepository.GetAsync(request.Id,d => new Dish() { Id = d.Id }, getDishByIdSpecification);

        if (entity is null) return;
        
        _dishRepository.Delete(entity);

        await unitOfWork.CompleteAsync();
    }
}