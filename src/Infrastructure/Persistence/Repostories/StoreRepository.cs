using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Persistence.Repositories;

internal class StoreRepository<TEntity>(StoreDbContext storeDbContext) 
    : Repository<TEntity, StoreDbContext>(storeDbContext)
    where TEntity : class, IEntity, IAggregateRoot;

