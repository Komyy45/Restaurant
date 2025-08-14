using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts;
using Restaurant.Infrastructure.Identity;
using Restaurant.Persistence.Data;
using Restaurant.Persistence.Identity;
using Serilog;

namespace Restaurant.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(jwtSettings =>
        {
            jwtSettings.Issuer = builder.Configuration["JWT:Issuer"]!;
            jwtSettings.Audience = builder.Configuration["JWT:Audience"]!;
            jwtSettings.Key = builder.Configuration["JWT:Key"]!;
        });

        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                    
                ClockSkew = TimeSpan.FromMinutes(0),
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
            };
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<RestaurantDbContext>();
        
        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));
        
        return builder;
    }
}