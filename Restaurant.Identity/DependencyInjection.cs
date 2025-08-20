using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts;
using Restaurant.Domain.Common;
using Restaurant.Identity.Common;
using Restaurant.Identity.Data;
using Restaurant.Identity.Entities;
using Restaurant.Identity.Policies.RestaurantOwnership;
using Restaurant.Identity.Services;

namespace Restaurant.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Start Identity DbContext Configurations //
        
        services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>();
        
        services.AddScoped<IIdentityDbContextInitializer, IdentityDbContextInitializer>();
        
        // End Identity DbContext Configurations //
        
        // Start Jwt Package Configuration //
        
        services.Configure<JwtSettings>(jwtSettings =>
        {
            jwtSettings.Issuer = configuration["JWT:Issuer"]!;
            jwtSettings.Audience = configuration["JWT:Audience"]!;
            jwtSettings.Key = configuration["JWT:Key"]!;
        });

        services.AddScoped<IAuthService, AuthService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                    
                ClockSkew = TimeSpan.FromMinutes(0),
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
            };
        });

        services.AddAuthorization(c =>
        {
            c.AddPolicy(PolicyTypes.RestaurantOwnership, builder =>
            {
                builder.RequireAuthenticatedUser();
                builder.AddRequirements(new RestaurantOwnershipRequirement(2));
            });
        });
        
        // End Jwt Package Configuration //

        // Start Identity Services //
        
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        
        // End Identity Services // 
        
        // Start Custom Policies Handlers //
        
        services.AddScoped<IAuthorizationHandler, RestaurantOwnershipRequirementHandler>();
        
        // End Custom Policies Handlers //
        
        return services;
    }
}