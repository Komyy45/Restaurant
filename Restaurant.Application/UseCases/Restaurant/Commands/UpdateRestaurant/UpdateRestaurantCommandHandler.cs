using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Application.UseCases.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.UseCases.Restaurant.Commands.UpdateRestaurant;

internal sealed class UpdateRestaurantCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeleteRestaurantCommand> logger) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Restaurant from the database.");
        var entity = await _restaurantRepository.GetAsync(request.Id);
        
        if (entity is null) return false;

        logger.LogInformation("Updating Restaurant.");

        entity = request.ToEntity(entity);

        _restaurantRepository.Update(entity);

        var rowsEffected = await unitOfWork.CompleteAsync();

        return rowsEffected > 0;
    }
}