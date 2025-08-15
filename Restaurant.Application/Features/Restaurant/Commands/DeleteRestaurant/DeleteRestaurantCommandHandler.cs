using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;

internal sealed class DeleteRestaurantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _restaurantRepository.GetAsync(request.Id);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));
        
        _restaurantRepository.Delete(entity);

        await unitOfWork.CompleteAsync();
    }
}