using FluentValidation;

namespace Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;

internal sealed class CreateRestaurantDishCommandValidator : AbstractValidator<CreateRestaurantDishCommand>
{
    public CreateRestaurantDishCommandValidator()
    {
        RuleFor(r => r.Price)
            .GreaterThan(0)
            .WithMessage("Price must be a positive value.");
        
        RuleFor(r => r.KiloCalories)
            .GreaterThan(0)
            .WithMessage("KiloCalories must be a positive value.");
    }
}