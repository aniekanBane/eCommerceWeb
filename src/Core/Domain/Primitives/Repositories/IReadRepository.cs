using Ardalis.Specification;
using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Primitives.Repositories;

public interface IReadRepository<TEntity> : IReadRepositoryBase<TEntity> 
    where TEntity : class, IEntity, IAggregateRoot
{
    Task<int> CountAsync<TUnMapped>(
        FormattableString @sql, 
        CancellationToken cancellationToken= default
    );

    Task<TUnMapped?> FirstOrDefaultAsync<TUnMapped>(
        FormattableString @sql, 
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TUnMapped>> ListAsync<TUnMapped>(
        FormattableString @sql, 
        CancellationToken cancellationToken= default
    );
}
