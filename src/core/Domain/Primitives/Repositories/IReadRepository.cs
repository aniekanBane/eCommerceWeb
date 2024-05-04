using Ardalis.Specification;
using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Primitives.Repositories;

public interface IReadRepository<TEntity> : IReadRepositoryBase<TEntity> 
    where TEntity : class, IEntity, IAggregateRoot
{
    Task<TUmapped?> GetBySqlQueryAsync<TUmapped>(
        FormattableString sql, 
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TUnMapped>> SqlQueryListAsync<TUnMapped>(
        FormattableString sql, 
        CancellationToken cancellationToken = default
    );
}
