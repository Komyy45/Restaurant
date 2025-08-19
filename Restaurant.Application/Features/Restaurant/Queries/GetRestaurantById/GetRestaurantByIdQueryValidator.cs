using FluentValidation;

namespace Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;

internal sealed class GetRestaurantByIdQueryValidator : AbstractValidator<GetRestaurantByIdQuery>
{
    public GetRestaurantByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive value.");
    }
}