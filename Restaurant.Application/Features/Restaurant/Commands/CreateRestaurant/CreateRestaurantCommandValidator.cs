using FluentValidation;

namespace Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;

internal sealed class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly string[] _validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    
    public CreateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .Length(3, 100);
            
        RuleFor(r => r.ContactEmail)
            .EmailAddress()
            .WithMessage("Please, Provide valid email address.");

        RuleFor(r => r.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please, Provide valid Postal code.");

        RuleFor(r => r.Category)
            .Must(category => _validCategories.Contains(category))
            .WithMessage("Invalid Category. Please Choose from the valid categories.");
    }
}