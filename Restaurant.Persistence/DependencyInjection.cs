using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Contracts;
using Restaurant.Persistence.Data;

namespace Restaurant.Persistence;
public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<RestaurantDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IDbContextInitializer, DbContextInitializer>();

		return services;
	}
}

