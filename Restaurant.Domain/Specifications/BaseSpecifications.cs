using System.Linq.Expressions;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Specifications;

public abstract class BaseSpecifications<TEntity, TKey> : ISpecification<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public Expression<Func<TEntity, bool>> Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; set; }

    public BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public BaseSpecifications()
    { }

    protected virtual void AddIncludes() => Includes = new();
    

}