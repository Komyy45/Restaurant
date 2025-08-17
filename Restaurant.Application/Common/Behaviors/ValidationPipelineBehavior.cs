using FluentValidation;
using MediatR;
using ValidationException = Restaurant.Domain.Exceptions.ValidationException;

namespace Restaurant.Application.Common.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = new();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);

            foreach (var error in result.Errors)
            {
                errors.Add($"{error.ErrorCode}_{error.PropertyName}", error.ErrorMessage);
            }
        }

        if (errors.Count > 0)
            throw new ValidationException(errors);
        
        var response = await next(cancellationToken);

        return response;
    }
}