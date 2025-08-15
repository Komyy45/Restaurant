namespace Restaurant.Domain.Exceptions;

public sealed class NotFoundException(object key, string entityName)
    : Exception($"{entityName} with key '{key}' was not found.");