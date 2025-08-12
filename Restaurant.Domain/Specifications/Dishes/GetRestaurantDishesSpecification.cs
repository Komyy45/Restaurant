using System.Linq.Expressions;

namespace Restaurant.Domain.Specifications.Dishes;
using RestaurantEntity = Entities.Restaurant;

public class GetRestaurantDishesSpecification : BaseSpecifications<RestaurantEntity, int>
{
    public GetRestaurantDishesSpecification(Expression<Func<RestaurantEntity, bool>> criteria) : base(criteria)
    {
        AddIncludes();
    }

    public GetRestaurantDishesSpecification() : base()
    {
        AddIncludes();
    }

    protected override void AddIncludes()
    {
        base.AddIncludes();
        Includes.Add(r => r.Dishes);
    }
}