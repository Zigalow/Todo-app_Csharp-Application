using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;

namespace Todo.Api.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly TodoDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(TodoDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public virtual Task<IEnumerable<T>> GetAllAsync(string id)
    {
        throw new NotImplementedException(
            $"GetAllAsync must be implemented in the derived repository for {typeof(T).Name}"
        );
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task<bool> ExistsAsync(int id)
    {
        return DbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }
}