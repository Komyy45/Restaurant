namespace Restaurant.Domain.Exceptions;

public sealed class OperationForbiddenException() : Exception("You are not allowed to perform this operation.");