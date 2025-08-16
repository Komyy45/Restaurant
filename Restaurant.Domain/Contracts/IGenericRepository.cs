using System.Linq.Expressions;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Contracts;

public interface IGenericRepository<TEntity, TKey>
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
{
	Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true);
	Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec, bool asNoTracking = true);
	Task<IEnumerable<TSelector>> GetAllAsync<TSelector>(Expression<Func<TEntity, TSelector>> selector, bool asNoTracking = true);	
	Task<IEnumerable<TSelector>> GetAllAsync<TSelector>(Expression<Func<TEntity, TSelector>> selector, ISpecification<TEntity, TKey> spec,  bool asNoTracking = true);
	Task<TEntity?> GetAsync(TKey key);
	Task<TSelector?> GetAsync<TSelector>(TKey key, Expression<Func<TEntity, TSelector>> selector);	
	Task<TEntity?> GetAsync(TKey key, ISpecification<TEntity, TKey> spec);
	Task<TSelector?> GetAsync<TSelector>(TKey key, Expression<Func<TEntity, TSelector>> selector, ISpecification<TEntity, TKey> spec);
	Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
	ValueTask CreateAsync(TEntity entity);
	void Update(TEntity entity);
	void Delete(TEntity entity);
}

