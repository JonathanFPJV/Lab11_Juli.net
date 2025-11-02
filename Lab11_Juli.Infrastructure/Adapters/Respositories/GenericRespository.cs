using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Juli.Infrastructure.Adapters.Respositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly TicketerabdContext _context;

    public GenericRepository(TicketerabdContext context)
    {
        _context = context;
    }
    public async Task<List<T>> GetAll() => await _context.Set<T>().ToListAsync();

    public async Task<T?> GetById(Guid id) => await _context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task Delete(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}