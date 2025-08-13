using FluentValidation;

namespace Restaurant.Application.UseCases.Dishes.Commands.CreateRestaurantDish;

internal sealed class CreateRestaurantDishCommandValidator : AbstractValidator<CreateRestaurantDishCommand>
{
    public CreateRestaurantDishCommandValidator()
    {
        RuleFor(r => r.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a positive value.");
        
        RuleFor(r => r.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a positive value.");
    }
}