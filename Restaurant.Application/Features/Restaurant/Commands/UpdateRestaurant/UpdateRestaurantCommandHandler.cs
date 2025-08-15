using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;

internal sealed class UpdateRestaurantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _restaurantRepository.GetAsync(request.Id);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));

        entity = request.ToEntity(entity);

        _restaurantRepository.Update(entity);

        await unitOfWork.CompleteAsync();
    }
}