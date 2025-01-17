using System.Data;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Repositories;
using eCommerceWeb.Domain.ValueObjects;
using eCommerceWeb.Persistence.Configurations;
using eCommerceWeb.Persistence.Conventions;
using eCommerceWeb.Persistence.Converters;
using eCommerceWeb.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Storage;

namespace eCommerceWeb.Persistence;

public sealed class StoreDbContext(DbContextOptions<StoreDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction is not null;

    public async Task<IDisposable?> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            return null;
        }

        _currentTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction is null)
            {
                throw new InvalidOperationException("No active transaction to commit");
            }

            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction!.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Conventions.Add(_ => new DirectoryConvention());

        configurationBuilder.Properties<EmailAddress>()
            .HaveConversion<EmailAddressConverter>();

        configurationBuilder.Properties<PhoneNumber>()
            .HaveConversion<PhoneNumberConverter>();

        configurationBuilder.Properties<Percent>()
            .HaveConversion<PercentConverter>();
            
        configurationBuilder.Properties<Money>()
            .HaveConversion<MoneyConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Catalog
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());

        // Customer
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());

        // Misc
        modelBuilder.ApplyConfiguration(new MediaFileConfiguration());

        // Directory
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
        
        modelBuilder.ApplyGlobalFilter<ISoftDeleteEntity>(e => !e.IsDeleted);
    }

    public override void Dispose()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null;
        base.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
        await base.DisposeAsync();
    }
}
