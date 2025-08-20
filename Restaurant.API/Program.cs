using Restaurant.API.Extensions;
using Restaurant.API.Middlewares;
using Restaurant.Identity.Common;
using Restaurant.Persistence.Data;
using Serilog;

namespace Restaurant.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.ConfigureServices();

			var app = builder.Build();

			await app.InitializeAsync<IIdentityDbContextInitializer>();

			await app.InitializeAsync<IRestaurantDbContextInitializer>();
			
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

			app.UseAuthentication();

			app.UseAuthorization();
			
			app.MapControllers();

			await app.RunAsync();
		}
	}
}
