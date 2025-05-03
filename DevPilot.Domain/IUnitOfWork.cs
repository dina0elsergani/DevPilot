namespace DevPilot.Domain;

/// <summary>
/// Unit of Work interface for managing transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
} 