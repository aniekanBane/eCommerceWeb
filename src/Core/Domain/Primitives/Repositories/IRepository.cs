using System.Linq.Expressions;
using Ardalis.Specification;
using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Primitives.Repositories;

public interface IRepository<TEntity> : IRepositoryBase<TEntity> 
    where TEntity : class, IEntity, IAggregateRoot
{
    Task<int> ExecuteDeleteAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    );
}
