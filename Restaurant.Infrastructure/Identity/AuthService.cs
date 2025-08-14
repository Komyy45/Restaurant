using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts;
using Restaurant.Application.UseCases.Authentication.Commands.LoginCommand;
using Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;
using Restaurant.Application.UseCases.Authentication.Commands.UpdateAccount;
using Restaurant.Application.UseCases.Authentication.Dtos;
using Restaurant.Domain.Exceptions;
using Restaurant.Infrastructure.Mapping;
using Restaurant.Persistence.Identity;
using ValidationException = Restaurant.Domain.Exceptions.ValidationException;

namespace Restaurant.Infrastructure.Identity;

internal sealed class AuthService(
    SignInManager<ApplicationUser> signInManager, 
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<JwtSettings> jwtSettingsOptions,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettingsOptions.Value;

    public async Task<AuthResponseDto> LoginAsync(LoginCommand loginCommand)
    {
        logger.LogInformation("Login attempt for email: {Email}", loginCommand.Email);

        var user = await userManager.FindByEmailAsync(loginCommand.Email);

        if (user is null)
        {
            logger.LogWarning("Login failed: No account found for email: {Email}", loginCommand.Email);
            throw new NotFoundException(loginCommand.Email, nameof(ApplicationUser));
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginCommand.Password, true);

        if (result.IsLockedOut)
        {
            logger.LogWarning("Login failed: Account locked for user {UserId}", user.Id);
            throw new AccountLockedException();
        }
        else if (result.RequiresTwoFactor)
        {
            logger.LogInformation("Login requires 2FA for user {UserId}", user.Id);
            throw new NotImplementedException();
        }
        else if (result.IsNotAllowed)
        {
            logger.LogWarning("Login not allowed for user {UserId}", user.Id);
            throw new IsNotAllowedException();
        }

        logger.LogInformation("Login successful for user {UserId}", user.Id);

        return new AuthResponseDto(
            AccessToken: await GenerateToken(user),
            RefreshToken: await GenerateRefreshToken(user)
        );
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterCommand registerCommand)
    {
        logger.LogInformation("Registering new user with email: {Email}", registerCommand.Email);

        var user = registerCommand.ToEntity();
            
        var result = await userManager.CreateAsync(user, registerCommand.Password);
        
        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            logger.LogWarning("Registration failed for email: {Email}. Errors: {@Errors}", registerCommand.Email, errors);
            throw new ValidationException(errors);
        }
        
        logger.LogInformation("Registration successful for user {UserId}", user.Id);

        return new AuthResponseDto(
            AccessToken: await GenerateToken(user),
            RefreshToken: await GenerateRefreshToken(user)
        );
    }

    public async Task<AccountDto> GetAccountById(string id)
    {
        logger.LogInformation("Fetching account for user with ID: {UserId}", id);

        var user = await userManager.FindByIdAsync(id);

        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found.", id);
            throw new NotFoundException(id, nameof(ApplicationUser));
        }

        var account = user.ToDto();

        logger.LogInformation("Successfully retrieved account for user ID: {UserId}", id);

        return account;
    }

    public async Task UpdateAccount(UpdateAccountCommand updateAccountCommand)
    {
        var user = await userManager.FindByIdAsync(updateAccountCommand.Id);

        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found for update.", updateAccountCommand.Id);
            throw new NotFoundException(updateAccountCommand.Id, nameof(ApplicationUser));
        }

        user = updateAccountCommand.ToEntity(user);

        var result = await userManager.UpdateAsync(user);

        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            logger.LogWarning("Update failed for user ID: {UserId}. Errors: {@Errors}", updateAccountCommand.Id, errors);
            throw new ValidationException(errors);
        }
    }


    private async Task<string> GenerateToken(ApplicationUser user)
    {   
        logger.LogDebug("Generating access token for user {UserId}", user.Id);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var userClaims = await userManager.GetClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);
        
        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); 
        userClaims.Add(new Claim(ClaimTypes.Email, user.Email!)); 
        userClaims.Add(new Claim(ClaimTypes.Name, user.UserName!));

        foreach (var role in userRoles)
            userClaims.Add(new Claim(ClaimTypes.Role, role));
        
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials);

        logger.LogDebug("Access token generated successfully for user {UserId}", user.Id);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Task<string> GenerateRefreshToken(ApplicationUser user)
    {
        logger.LogDebug("Generating refresh token for user {UserId}", user.Id);
        return Task.FromResult(Guid.NewGuid().ToString());
    }
}
