using System.Linq.Expressions;

namespace Restaurant.Domain.Specifications.Restaurants;
using RestaurantEntity = Entities.Restaurant;
public class GetAllRestaurantsSpecification : BaseSpecification<RestaurantEntity, int>
{
    private readonly Dictionary<string, Expression<Func<RestaurantEntity, object>>> _columnSelector =
        new()
        {
            {nameof(RestaurantEntity.Name), r =>  r.Name},
            {nameof(RestaurantEntity.Description), r => r.Description },
            {nameof(RestaurantEntity.Category), r => r.Category}
        };
    
    public GetAllRestaurantsSpecification(string? searchText, int pageSize, int pageNumber, string? sortBy, bool sortDirection) : base(r =>
        string.IsNullOrWhiteSpace(searchText) || 
        r.Name.Contains(searchText) ||
        r.Description.Contains(searchText))
    {
        ApplyPagination(pageSize, pageNumber);
        if(sortBy is not null)
            AddOrderBy(sortBy, sortDirection, _columnSelector);
    }
}