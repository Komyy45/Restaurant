using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;

internal sealed class UpdateRestaurantCommandHandler(IUnitOfWork unitOfWork,  
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.Id);

        if (restaurant is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));
        
        var isAuthorized = restaurantAuthorizationService.IsAuthorized(restaurant, ResourceOperation.Update);

        if (!isAuthorized) throw new OperationForbiddenException();

        restaurant = request.ToEntity(restaurant);

        _restaurantRepository.Update(restaurant);

        await unitOfWork.CompleteAsync();
    }
}