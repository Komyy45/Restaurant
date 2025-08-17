using System.Linq.Expressions;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Contracts;

public interface ISpecification<TEntity, TKey>
where TEntity : BaseEntity<TKey>
where TKey : IEquatable<TKey>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>>? Includes { get; set; }
    public bool IsPaginationEnabled { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool SortDirection { get; set; }
    public Expression<Func<TEntity, object>>? OrderBy { get; set; }
}