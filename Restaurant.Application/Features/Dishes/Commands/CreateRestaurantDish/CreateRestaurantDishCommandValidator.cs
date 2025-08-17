using FluentValidation;

namespace Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;

internal sealed class CreateRestaurantDishCommandValidator : AbstractValidator<CreateRestaurantDishCommand>
{
    public CreateRestaurantDishCommandValidator()
    {
        RuleFor(d => d.Name)
            .Length(3, 100);
        
        RuleFor(d => d.Description)
            .MaximumLength(300);
        
        RuleFor(d => d.Price)
            .GreaterThan(0)
            .WithMessage("Price must be a positive value.");
        
        RuleFor(d => d.KiloCalories)
            .GreaterThan(0)
            .WithMessage("KiloCalories must be a positive value.");
    }
}