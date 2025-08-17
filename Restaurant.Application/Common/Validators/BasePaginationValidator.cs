using FluentValidation;
using Restaurant.Application.Common.Messaging;

namespace Restaurant.Application.Common.Validators;

internal abstract class BasePaginationValidator<TRequest> : AbstractValidator<TRequest>
where TRequest : BasePaginatedQuery
{
    private readonly IReadOnlyList<int> _allowedPageSizes =
    [
        5,
        10,
        15,
        30
    ];
    
    
    protected BasePaginationValidator(IReadOnlyList<string> allowedSortByColumnNames)
    {
        RuleFor(r => r.PageNumber)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(r => r.PageSize)
            .NotEmpty()
            .Must(pageSize => _allowedPageSizes.Contains(pageSize))
            .WithMessage($"Page size must be in [{string.Join(", ", _allowedPageSizes)}]");
        
        RuleFor(r => r.SortBy)
            .Must(columnName => string.IsNullOrEmpty(columnName) || allowedSortByColumnNames.Contains(columnName))
            .WithMessage($"SortBy is optional, or must be in [{string.Join(", ", allowedSortByColumnNames)}]");
    }
}