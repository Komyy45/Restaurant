using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Specifications.Dishes;

public class GetDishByIdSpecification : BaseSpecification<Dish, int>
{
    public GetDishByIdSpecification(int restaurantId) : base(d => d.RestaurantId == restaurantId)
    {
        
    }
}