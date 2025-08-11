
using Restaurant.Application.Contracts;
using Restaurant.Persistence;

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

			builder.Services.AddPersistenceServices(builder.Configuration);

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi(); 

			#endregion

			var app = builder.Build();

			var scope = app.Services.CreateScope();

			var dbContextInitializer = scope.ServiceProvider.GetRequiredService<IDbContextInitializer>();

			try
			{
				await dbContextInitializer.InitializeAsync();
				await dbContextInitializer.SeedAsync();
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

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
