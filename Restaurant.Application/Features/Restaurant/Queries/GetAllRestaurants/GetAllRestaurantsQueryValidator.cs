using FluentValidation;
using Restaurant.Application.Common.Validators;

namespace Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;
using RestaurantEntity = Domain.Entities.Restaurant;

internal sealed class GetAllRestaurantsQueryValidator : BasePaginationValidator<GetAllRestaurantsQuery>
{
    private static readonly IReadOnlyList<string> AllowedSortByColumnNames = [
        nameof(RestaurantEntity.Name), 
        nameof(RestaurantEntity.Description),
        nameof(RestaurantEntity.Category)
    ];
    
    public GetAllRestaurantsQueryValidator() : base(AllowedSortByColumnNames)
    { }
}