﻿using System.Linq.Expressions;
using Ardalis.Specification;
using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Primitives.Repositories;

public interface IWriteRepository<TEntity> : IRepositoryBase<TEntity> 
    where TEntity : class, IEntity, IAggregateRoot
{
    Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    );
}
