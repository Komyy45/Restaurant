using Microsoft.OpenApi.Models;
using Restaurant.API.Middlewares;
using Restaurant.Application;
using Restaurant.Application.Contracts;
using Restaurant.Infrastructure;
using Restaurant.Persistence;
using Serilog;

namespace Restaurant.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			#region Configure Services

			builder.Services.AddControllers();

			builder.Services.AddPersistenceServices(builder.Configuration)
							.AddApplicationServices();
			
			builder.AddInfrastructureServices();

			builder.Services.AddSingleton<GlobalExceptionHandlingMiddleware>();
			
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant.API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
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


			#endregion

			var app = builder.Build();

			var scope = app.Services.CreateScope();

			try
			{
				var dbContextInitializer = scope.ServiceProvider.GetService<IDbContextInitializer>();

				if (dbContextInitializer is not null)
				{
					await dbContextInitializer.InitializeAsync();
					await dbContextInitializer.SeedAsync();
				}

			}
			catch (Exception ex)
			{
				var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

				logger.LogError(ex.Message);
			}


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
				app.UseReDoc(c =>
				{
					c.DocumentTitle = "REDOC API Documentation";
					c.SpecUrl = "api/document.json";
				});
			}

			app.UseSerilogRequestLogging();
			
			app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
