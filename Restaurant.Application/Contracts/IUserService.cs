using Restaurant.Application.Common.User;

namespace Restaurant.Application.Contracts;

public interface IUserService
{
    public CurrentUser GetCurrentUser();
}