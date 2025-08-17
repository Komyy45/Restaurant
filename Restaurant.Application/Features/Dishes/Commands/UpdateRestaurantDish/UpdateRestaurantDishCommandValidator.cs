using FluentValidation;

namespace Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;

internal sealed class UpdateRestaurantDishCommandValidator : AbstractValidator<UpdateRestaurantDishCommand>
{
    public UpdateRestaurantDishCommandValidator()
    {
        RuleFor(d => d.Name)
            .Length(3, 100);
        
        RuleFor(d => d.Description)
            .MaximumLength(300);
        
        RuleFor(r => r.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a positive value.");
        
        RuleFor(r => r.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a positive value.");
    }
}