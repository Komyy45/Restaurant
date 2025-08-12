using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Persistence.Repositories;

internal static class SpecificationEvaluator
{
    internal static IQueryable<TEntity> Evaluate<TEntity, TKey>(this IQueryable<TEntity> query, ISpecification<TEntity, TKey> spec) 
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        if (spec.Includes is not null)
            query = spec.Includes.Aggregate(query, (acc, include) => acc.Include(include));
        
        return query;
    }

}