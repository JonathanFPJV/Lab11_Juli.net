namespace Lab11_Juli.Domain.Ports.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    
    Task<int> Complete();
}