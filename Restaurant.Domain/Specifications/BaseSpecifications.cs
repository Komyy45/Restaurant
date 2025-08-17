using System.Linq.Expressions;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Specifications;

public abstract class BaseSpecifications<TEntity, TKey> : ISpecification<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>>? Includes { get; set; }
    public bool IsPaginationEnabled { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    // true == Descending  
    // false == Ascending
    public bool SortDirection { get; set; }
    public Expression<Func<TEntity, object>>? OrderBy { get; set; }

    public BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public BaseSpecifications()
    { }

    protected virtual void AddIncludes() => Includes = new();

    protected void ApplyPagination(int pageSize, int pageNumber)
    {
        IsPaginationEnabled = true;
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
    }

    protected void AddOrderBy(string column, bool sortDirection, Dictionary<string, Expression<Func<TEntity, object>>> columnSelector)
    {
        OrderBy = columnSelector[column];
        SortDirection = sortDirection;
    }
}