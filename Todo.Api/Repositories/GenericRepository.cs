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

    public async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id)
    {
        return DbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }
}