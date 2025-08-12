using System.Linq.Expressions;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Contracts;

public interface ISpecification<TEntity, TKey>
where TEntity : BaseEntity<TKey>
where TKey : IEquatable<TKey>
{
    public Expression<Func<TEntity, bool>> Criteria { get; set; }
    
    public List<Expression<Func<TEntity, object>>> Includes { get; set; }
}