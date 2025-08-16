using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Commands.Login;
using Restaurant.Application.Features.Authentication.Commands.RefreshToken;
using Restaurant.Application.Features.Authentication.Commands.Register;
using Restaurant.Application.Features.Authentication.Commands.RevokeToken;
using Restaurant.Application.Features.Authentication.Commands.UpdateAccount;
using Restaurant.Application.Features.Authentication.Models.Responses;
using Restaurant.Domain.Exceptions;
using Restaurant.Infrastructure.Common;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Mapping;
using ValidationException = Restaurant.Domain.Exceptions.ValidationException;

namespace Restaurant.Infrastructure.Services;

internal sealed class AuthService(
    SignInManager<ApplicationUser> signInManager, 
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettingsOptions, 
    IdentityDbContext identityDbContext,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettingsOptions.Value;

    public async Task<AuthResponse> LoginAsync(LoginCommand loginCommand)
    {
        logger.LogInformation("Executing LoginAsync for email: {Email}", loginCommand.Email);

        var user = await userManager.FindByEmailAsync(loginCommand.Email);

        if (user is null)
            throw new NotFoundException(loginCommand.Email, nameof(ApplicationUser));

        var result = await signInManager.CheckPasswordSignInAsync(user, loginCommand.Password, true);

        if (result.IsLockedOut) throw new AccountLockedException();
        if (result.RequiresTwoFactor) throw new NotImplementedException();
        if (result.IsNotAllowed) throw new IsNotAllowedException();

        var claims = await GetUserClaims(user);

        var refreshToken = GenerateRefreshToken();

        var token = await identityDbContext.RefreshTokens
            .FirstOrDefaultAsync(token => token.UserId == user.Id);
        

        if (token is null)
        {
            identityDbContext.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
        }
        else
        {
            token.Token = refreshToken;
            token.ExpiresAt = DateTime.UtcNow.AddDays(7);
        }
        
        
        await identityDbContext.SaveChangesAsync();
        
        return new AuthResponse(
            AccessToken: GenerateToken(claims),
            RefreshToken: refreshToken
        );
    }

    public async Task<AuthResponse> RegisterAsync(RegisterCommand registerCommand)
    {
        logger.LogInformation("Executing RegisterAsync for email: {Email}", registerCommand.Email);

        var user = registerCommand.ToEntity();
        var result = await userManager.CreateAsync(user, registerCommand.Password);

        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            throw new ValidationException(errors);
        }

        var claims = await GetUserClaims(user);

        var refreshToken = GenerateRefreshToken();

        identityDbContext.RefreshTokens.Add(new RefreshToken()
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await identityDbContext.SaveChangesAsync();
        
        return new AuthResponse(
            AccessToken: GenerateToken(claims),
            RefreshToken: refreshToken
        );
    }

    public async Task<AccountResponse> GetAccountById(string id)
    {
        logger.LogInformation("Executing GetAccountById for userId: {UserId}", id);

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            throw new NotFoundException(id, nameof(ApplicationUser));

        return user.ToDto();
    }

    public async Task UpdateAccount(UpdateAccountCommand updateAccountCommand)
    {
        logger.LogInformation("Executing UpdateAccount for userId: {UserId}", updateAccountCommand.Id);

        var user = await userManager.FindByIdAsync(updateAccountCommand.Id);
        if (user is null)
            throw new NotFoundException(updateAccountCommand.Id, nameof(ApplicationUser));

        user = updateAccountCommand.ToEntity(user);
        var result = await userManager.UpdateAsync(user);

        if (result.Errors.Any())
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            throw new ValidationException(errors);
        }
    }

    public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand refreshTokenCommand)
    {
        logger.LogInformation("Executing RefreshToken");

        var principal = GetPrincipalFromExpiredToken(refreshTokenCommand.AccessToken);
        var refreshToken = await identityDbContext.RefreshTokens.FirstOrDefaultAsync(token =>
            token.Token == refreshTokenCommand.RefreshToken &&
            token.UserId == principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (refreshToken is null)
            throw new NotFoundException(refreshTokenCommand.RefreshToken, nameof(RefreshToken));

        if (refreshToken.IsExpired)
            throw new UnauthorizedAccessException();

        refreshToken.Token = GenerateRefreshToken();
        refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(7);

        await identityDbContext.SaveChangesAsync();

        string accessToken = GenerateToken(principal.Claims);

        return new RefreshTokenResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token
        );
    }

    public async Task RevokeToken(RevokeTokenCommand revokeTokenCommand)
    {
        logger.LogInformation("Executing RevokeToken");

        int numberOfDeletedRows = await identityDbContext.RefreshTokens
            .Where(token => revokeTokenCommand.RefreshToken == token.Token)
            .ExecuteDeleteAsync();

        if (numberOfDeletedRows <= 0)
            throw new NotFoundException(revokeTokenCommand.RefreshToken, nameof(RefreshToken));
    }

    private async Task<IList<Claim>> GetUserClaims(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);
        
        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); 
        userClaims.Add(new Claim(ClaimTypes.Email, user.Email!)); 
        userClaims.Add(new Claim(ClaimTypes.Name, user.UserName!));
        
        foreach (var role in userRoles)
            userClaims.Add(new Claim(ClaimTypes.Role, role));

        return userClaims;
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        logger.LogDebug("Generating access token for user {UserId}", userId);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        logger.LogDebug("Access token generated successfully for user {UserId}", userId);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
                    
            ClockSkew = TimeSpan.FromMinutes(0),
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
