using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;

internal sealed class DeleteRestaurantCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeleteRestaurantCommand> logger) : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        
        logger.LogInformation("Getting Restaurant with Id: {@id}.", request.Id);
        
        var entity = await _restaurantRepository.GetAsync(request.Id);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));
        
        logger.LogInformation("Deleting the Restaurant from the Database.");
        
        _restaurantRepository.Delete(entity);

        await unitOfWork.CompleteAsync();
    }
}