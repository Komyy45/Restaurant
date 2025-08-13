
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

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi(); 

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
				app.MapOpenApi();
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
