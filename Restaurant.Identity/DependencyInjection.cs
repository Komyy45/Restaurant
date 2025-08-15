using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts;
using Restaurant.Infrastructure.Common;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Services;

namespace Restaurant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("IdentityConnection")));
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>();
        
        services.AddScoped<IIdentityDbContextInitializer, IdentityDbContextInitializer>();
        
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

       

        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}