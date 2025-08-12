using FluentValidation;

namespace Restaurant.Application.UseCases.Dishes.Commands.UpdateRestaurantDish;

internal sealed class UpdateRestaurantDishCommandValidator : AbstractValidator<UpdateRestaurantDishCommand>
{
    public UpdateRestaurantDishCommandValidator()
    {
        RuleFor(r => r.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a positive value.");
        
        RuleFor(r => r.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a positive value.");
    }
}