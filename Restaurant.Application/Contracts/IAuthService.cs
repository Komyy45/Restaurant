using Restaurant.Application.Features.Authentication.Commands.Login;
using Restaurant.Application.Features.Authentication.Commands.RefreshToken;
using Restaurant.Application.Features.Authentication.Commands.Register;
using Restaurant.Application.Features.Authentication.Commands.RevokeToken;
using Restaurant.Application.Features.Authentication.Commands.UpdateAccount;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Contracts;

public interface IAuthService
{
    public Task<AuthResponse> LoginAsync(LoginCommand loginCommand);
    public Task<AuthResponse> RegisterAsync(RegisterCommand registerCommand);
    public Task<AccountResponse> GetAccountById(string id);
    public Task UpdateAccount(UpdateAccountCommand updateAccountCommand);
    public Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand refreshTokenCommand);
    public Task RevokeToken(RevokeTokenCommand revokeTokenCommand);
}