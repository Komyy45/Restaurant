using FluentValidation;
using Restaurant.Application.Common.Validators;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;

internal sealed class GetRestaurantDishesQueryValidator : BasePaginationValidator<GetRestaurantDishesQuery>
{
    private static readonly IReadOnlyList<string> AllowedSortByColumnNames = [
        nameof(Dish.Name), 
        nameof(Dish.Description),
        nameof(Dish.Price),
        nameof(Dish.KiloCalories),
    ];
    
    public GetRestaurantDishesQueryValidator() : base(AllowedSortByColumnNames)
    {
    }
}