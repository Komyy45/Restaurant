using FluentValidation;

namespace Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;

internal sealed class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    private readonly string[] _validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .Length(3, 100);
        
        RuleFor(r => r.Description)
            .MaximumLength(300);
    }
}