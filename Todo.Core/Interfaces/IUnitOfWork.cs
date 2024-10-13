using Todo.Core.Entities;

namespace Todo.Core.Interfaces;

public interface IUnitOfWork
{
    IRepository<ApplicationUser> Users { get; }
    IRepository<TodoItem> TodoItems { get; }
    IRepository<TodoList> TodoLists { get; }
    IRepository<Project> Projects { get; }
    IRepository<Label> Labels { get; }
    IRepository<ApplicationRole> Roles { get; }
    IRepository<PermissionType> Permissions { get; }
    IRepository<ProjectCollaborators> UserProjectAssignments { get; }

    Task SaveChangesAsync();
}