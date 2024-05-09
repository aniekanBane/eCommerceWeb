using System.Data;

namespace eCommerceWeb.Domain.Primitives.Repositories;

public interface IUnitOfWork
{
    Task<IDisposable?> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default
    );

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
