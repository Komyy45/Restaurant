using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Specifications.Dishes;

public class GetDishByIdSpecification : BaseSpecifications<Dish, int>
{
    public GetDishByIdSpecification(int restaurantId) : base(d => d.RestaurantId == restaurantId)
    {
        
    }
}