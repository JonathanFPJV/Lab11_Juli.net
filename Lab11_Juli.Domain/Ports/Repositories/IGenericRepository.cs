namespace Lab11_Juli.Domain.Ports.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAll();
    Task<T?> GetById(Guid id);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(Guid id);
    Task AddAsync(T entity);
}