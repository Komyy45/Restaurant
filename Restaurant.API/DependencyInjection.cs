using Microsoft.OpenApi.Models;
using Restaurant.API.Middlewares;
using Restaurant.Application;
using Restaurant.Infrastructure;
using Restaurant.Persistence;

namespace Restaurant.API;

public static class DependencyInjection
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddPersistenceServices(builder.Configuration)
            .AddApplicationServices()
            .AddIdentityServices(builder.Configuration);
			
        builder.AddInfrastructureServices();

        builder.Services.AddSingleton<GlobalExceptionHandlingMiddleware>();
			
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant.API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token in this format: Bearer {token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddHttpContextAccessor();
    }
}