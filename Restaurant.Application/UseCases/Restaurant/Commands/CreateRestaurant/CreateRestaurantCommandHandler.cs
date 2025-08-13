using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateRestaurantCommandHandler> logger) : IRequestHandler<CreateRestaurantCommand, int>
{
    private IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository = unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new Restaurant {@restaurant}.", request);
        var restaurant = request.ToEntity();

        logger.LogInformation("Creating new Restaurant.");
        
        await _restaurantRepository.CreateAsync(restaurant);
        
        await unitOfWork.CompleteAsync();
        
        return restaurant.Id;
    }
}