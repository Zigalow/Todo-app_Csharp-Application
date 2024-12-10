namespace Todo.Api.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync(string userId);
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(int id);
}