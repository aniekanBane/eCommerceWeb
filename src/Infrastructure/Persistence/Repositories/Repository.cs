using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Repositories;

namespace eCommerceWeb.Persistence.Repositories;

internal abstract class Repository<TEntity, TDbContext>(TDbContext dbContext)
    : RepositoryBase<TEntity>(dbContext), IReadRepository<TEntity>, IRepository<TEntity>
    where TEntity : class, IEntity, IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
{
    protected readonly DbSet<TEntity> dbSet = dbContext.Set<TEntity>();

    public async Task<int> ExecuteDeleteAsync(
        Expression<Func<TEntity, bool>> filter, 
        CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(filter).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int> CountAsync<TUnMapped>(
        FormattableString sql, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Database
            .SqlQuery<TUnMapped>(sql)
            .CountAsync(cancellationToken);
    }

    public async Task<TUnMapped?> FirstOrDefaultAsync<TUnMapped>(
        FormattableString sql, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Database
            .SqlQuery<TUnMapped>(sql)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TUnMapped>> ListAsync<TUnMapped>(
        FormattableString sql, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Database
            .SqlQuery<TUnMapped>(sql)
            .ToListAsync(cancellationToken);
    }

    #region overrides
    
    public override async Task<TEntity> AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public override async Task<IEnumerable<TEntity>> AddRangeAsync(
        IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        await dbSet.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public override Task DeleteAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public override Task DeleteRangeAsync(
        IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public override Task DeleteRangeAsync(
        ISpecification<TEntity> specification, 
        CancellationToken cancellationToken = default)
    {
        dbSet.RemoveRange(ApplySpecification(specification));
        return Task.CompletedTask;
    }

    public override Task UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        dbSet.Update(entity);
        return base.UpdateAsync(entity, cancellationToken);
    }

    public override Task UpdateRangeAsync(
        IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    #endregion
}
