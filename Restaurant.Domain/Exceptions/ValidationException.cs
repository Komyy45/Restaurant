namespace Restaurant.Domain.Exceptions;

public sealed class ValidationException(Dictionary<string, string> errors) : Exception
{
    public Dictionary<string, string> Errors = errors;
}