using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;

namespace Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRestaurantCommand, int>
{
    private IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository = unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = request.ToEntity();

        await _restaurantRepository.CreateAsync(restaurant);
        
        await unitOfWork.CompleteAsync();
        
        return restaurant.Id;
    }
}