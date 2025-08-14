namespace Restaurant.Domain.Exceptions;

public sealed class AccountLockedException : Exception
{
    public AccountLockedException() 
        : base("The account is locked. Please contact support or try again later.")
    {
    }

    public AccountLockedException(string message) 
        : base(message)
    {
    }
}