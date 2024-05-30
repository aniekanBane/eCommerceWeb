using System.Data;
using System.Linq.Expressions;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Repositories;
using eCommerceWeb.Domain.ValueObjects;
using eCommerceWeb.Persistence.Converters;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace eCommerceWeb.Persistence;

public sealed class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options), IUnitOfWork
{
    private IDbContextTransaction? _dbContextTransaction;
    public bool HasActiveTransaction => _dbContextTransaction is not null;

    public async Task<IDisposable?> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default)
    {
        if (HasActiveTransaction) return null;

        _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _dbContextTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (!HasActiveTransaction)
            throw new DbUpdateException("Attemping to commit null transaction");

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _dbContextTransaction!.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            RollBackTransaction();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public void RollBackTransaction()
    {
        if (HasActiveTransaction)
        {
            _dbContextTransaction?.Rollback();
            DisposeTransaction();
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<EmailAddress>().HaveConversion<EmailAddressConverter>();
        configurationBuilder.Properties<Money>().HaveConversion<MoneyConverter>();
        configurationBuilder.Properties<Percent>().HaveConversion<PercentConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        modelBuilder.ApplyGlobalFilter<ISoftDeleteEntity>(e => !e.IsDeleted);
    }

    private void DisposeTransaction()
    {
        _dbContextTransaction?.Dispose();
        _dbContextTransaction = null;
    }
}

internal static class ModelBuilderExtensions
{
    public static void ApplyGlobalFilter<TType>(
        this ModelBuilder modelBuilder, 
        Expression<Func<TType, bool>> filter)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsAssignableTo(typeof(TType))))
        {
            var param = Expression.Parameter(entity.ClrType);
            var body = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), param, filter.Body);
            var expression = Expression.Lambda(body, param);

            entity.SetQueryFilter(expression);
        }
    }
}
