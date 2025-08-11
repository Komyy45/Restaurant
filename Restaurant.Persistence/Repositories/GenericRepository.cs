using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;
using Restaurant.Persistence.Data;

namespace Restaurant.Persistence.Repositories;
internal sealed class GenericRepository<TEntity, TKey>(RestaurantDbContext context) : IGenericRepository<TEntity, TKey>
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
{
	private DbSet<TEntity> dbSet = context.Set<TEntity>();

	public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true)
	{
		if(asNoTracking) return await dbSet.AsNoTracking().ToListAsync();
		return await dbSet.ToListAsync();
	}

	public async Task<IEnumerable<TSelector>> GetAllAsync<TSelector>(Expression<Func<TEntity, TSelector>> selector, bool asNoTracking = true)
	{
		if(asNoTracking) return await dbSet.AsNoTracking().Select(selector).ToListAsync();
		return await dbSet.Select(selector).ToListAsync();
	}
	public async Task<TEntity?> GetAsync(TKey key)
	{
		return await dbSet.FirstOrDefaultAsync(e => e.Id.Equals(key));
	}

	public async Task<TSelector?> GetAsync<TSelector>(TKey key, Expression<Func<TEntity, TSelector>> selector)
	{
		return await dbSet.Where(e => e.Id.Equals(key)).Select(selector).FirstOrDefaultAsync();
	}

	public async ValueTask CreateAsync(TEntity entity)
	{
		await dbSet.AddAsync(entity);
	}

	public void UpdateAsync(TEntity entity)
	{
		dbSet.Update(entity);
	}

	public void DeleteAsync(TEntity entity)
	{
		dbSet.Remove(entity);
	}

}

