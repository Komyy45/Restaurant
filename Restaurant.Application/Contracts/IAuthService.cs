using Restaurant.Application.UseCases.Authentication.Commands.LoginCommand;
using Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;
using Restaurant.Application.UseCases.Authentication.Commands.UpdateAccount;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.Contracts;

public interface IAuthService
{
    public Task<AuthResponseDto> LoginAsync(LoginCommand loginCommand);
    public Task<AuthResponseDto> RegisterAsync(RegisterCommand registerCommand);
    public Task<AccountDto> GetAccountById(string id);
    public Task UpdateAccount(UpdateAccountCommand updateAccountCommand);
}