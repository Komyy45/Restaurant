using System.Linq.Expressions;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Specifications.Dishes;

public class GetRestaurantDishesSpecification : BaseSpecifications<Dish, int>
{
    private readonly Dictionary<string, Expression<Func<Dish, object>>> _columnSelector =
        new()
        {
            {nameof(Dish.Name), dish => dish.Name},
            {nameof(Dish.Description), dish => dish.Description },
            {nameof(Dish.Price), dish => dish.Price},
            {nameof(Dish.KiloCalories), dish => dish.KiloCalories!},
        };
    
    public GetRestaurantDishesSpecification(string? searchText, int pageSize, int pageIndex, string? sortBy,
        bool sortDirection, int restaurantId) : base(dish =>
        restaurantId == dish.RestaurantId &&
        (string.IsNullOrWhiteSpace(searchText) ||
         dish.Name.Contains(searchText) ||
         dish.Description.Contains(searchText)))
    {
        ApplyPagination(pageSize, pageIndex);
        if(sortBy is not null)
            AddOrderBy(sortBy!, sortDirection, _columnSelector);
    }
}