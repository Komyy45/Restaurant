namespace Restaurant.Domain.Exceptions;

public sealed class IsNotAllowedException : Exception
{
    public IsNotAllowedException() 
        : base("This action is not allowed.")
    {
    }

    public IsNotAllowedException(string message) 
        : base(message)
    {
    }
}