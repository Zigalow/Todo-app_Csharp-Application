using Todo.Core.Entities;
using Label = System.Reflection.Emit.Label;

namespace Todo.Core.Repositories;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<TodoItem> TodoItems { get; }
    IRepository<TodoList> TodoLists { get; }
    IRepository<Project> Projects { get; }
    IRepository<Label> Labels { get; }
    IRepository<Role> Roles { get; }
    IRepository<PermissionType> Permissions { get; }
    IRepository<ProjectCollaborators> UserProjectAssignments { get; }

    Task SaveChangesAsync();
}