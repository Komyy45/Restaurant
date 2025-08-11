using System.Linq.Expressions;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Contracts;

public interface IGenericRepository<TEntity, TKey>
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
{
	Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true);
	Task<IEnumerable<TSelector>> GetAllAsync<TSelector>(Expression<Func<TEntity, TSelector>> selector, bool asNoTracking = true);
	Task<TEntity?> GetAsync(TKey key);
	Task<TSelector?> GetAsync<TSelector>(TKey key, Expression<Func<TEntity, TSelector>> selector);
	ValueTask CreateAsync(TEntity entity);
	void UpdateAsync(TEntity entity);
	void DeleteAsync(TEntity entity);
}

