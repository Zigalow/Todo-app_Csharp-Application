using Todo.Api.Data;
using Todo.Api.Interfaces;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TodoDbContext _dbContext;

    public IRepository<ApplicationUser> Users { get; }
    public ITodoItemRepository TodoItems { get; }
    public ITodoListRepository TodoLists { get; }
    public IProjectRepository Projects { get; }
    public ILabelRepository Labels { get; }
    public IRepository<ProjectCollaborators> ProjectCollaborators { get; }

    public UnitOfWork(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
        Users = new GenericRepository<ApplicationUser>(_dbContext);
        TodoItems = new TodoItemRepository(_dbContext);
        TodoLists = new TodoListRepository(_dbContext);
        Projects = new ProjectRepository(_dbContext);
        Labels = new LabelRepository(_dbContext);
        ProjectCollaborators = new GenericRepository<ProjectCollaborators>(_dbContext);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}