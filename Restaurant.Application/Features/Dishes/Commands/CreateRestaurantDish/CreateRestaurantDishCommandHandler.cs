using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;

internal sealed class CreateRestaurantDishCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRestaurantDishCommand, int>
{
    private readonly IGenericRepository<Dish, int> _dishRepository = unitOfWork.GetRepository<Dish, int>();
    
    public async Task<int> Handle(CreateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        var dish = request.ToEntity();
        
        await _dishRepository.CreateAsync(dish);

        await unitOfWork.CompleteAsync();

        return dish.Id;
    }
}