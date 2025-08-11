using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandHandler(IUnitOfWork unitOfWork,
    ILogger<CreateRestaurantCommandHandler> logger) : IRequestHandler<CreateRestaurantCommand, int>
{
    private IGenericRepository<Domain.Entities.Restaurant, int> _restaurantRepository = unitOfWork.GetRepository<Domain.Entities.Restaurant, int>();
    
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = new Domain.Entities.Restaurant
        {
            Id = 0,
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            HasDelivery = request.HasDelivery,
            ContactEmail = request.ContactEmail,
            Contactumber = request.ContactNumber,
            Address = new Address
            {
                City = request.City,
                Street = request.Street,
                PostalCode = request.Postalcode
            },
        };

        logger.LogInformation("Creating new Restaurant.");
        
        await _restaurantRepository.CreateAsync(restaurant);
        
        await unitOfWork.CompleteAsync();
        
        return restaurant.Id;
    }
}