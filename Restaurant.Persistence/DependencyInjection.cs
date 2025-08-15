using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Contracts;
using Restaurant.Domain.Contracts;
using Restaurant.Persistence.Data;
using Restaurant.Persistence.Repositories;

namespace Restaurant.Persistence;
public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<RestaurantDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
		
		services.AddScoped<IRestaurantDbContextInitializer, RestaurantDbContextInitializer>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		return services;
	}
}

