using Todo.Api.Data;
using Todo.Api.Interfaces.EntityInterfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories.EntityRepositories;

public class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(TodoDbContext dbContext) : base(dbContext)
    {
    }
    
    
    
}