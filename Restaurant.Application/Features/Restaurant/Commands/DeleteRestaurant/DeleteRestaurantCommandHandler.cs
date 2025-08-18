using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;

internal sealed class DeleteRestaurantCommandHandler(IUnitOfWork unitOfWork,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository =
        unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.Id);

        if (restaurant is null)
            throw new NotFoundException(request.Id, nameof(Restaurant));

        var isAuthorized = restaurantAuthorizationService.IsAuthorized(restaurant, ResourceOperation.Delete);

        if (!isAuthorized) throw new OperationForbiddenException();
        
        _restaurantRepository.Delete(restaurant);

        await unitOfWork.CompleteAsync();
    }
}