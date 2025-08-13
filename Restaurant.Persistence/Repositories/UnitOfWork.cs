using System.Collections.Concurrent;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;
using Restaurant.Persistence.Data;

namespace Restaurant.Persistence.Repositories;
internal sealed class UnitOfWork(RestaurantDbContext context) : IUnitOfWork
{
	private ConcurrentDictionary<string, object> repositories = new();

	public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
	{
		return (IGenericRepository<TEntity, TKey>)repositories.GetOrAdd(typeof(IGenericRepository<TEntity, TKey>).Name, new GenericRepository<TEntity, TKey>(context));
	}


	public async Task<int> CompleteAsync()
	{
		return await context.SaveChangesAsync();
	}

	public async ValueTask DisposeAsync()
	{
		await context.DisposeAsync();
	}

}
