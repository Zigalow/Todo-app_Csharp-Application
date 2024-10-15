using Todo.Api.Interfaces.EntityInterfaces;
using Todo.Core.Entities;

namespace Todo.Api.Interfaces;

public interface IUnitOfWork
{
    IRepository<ApplicationUser> Users { get; }
    IRepository<TodoItem> TodoItems { get; }
    IRepository<TodoList> TodoLists { get; }
    IProjectRepository Projects { get; }
    IRepository<Label> Labels { get; }
    IRepository<ApplicationRole> Roles { get; }
    IRepository<PermissionType> Permissions { get; }
    IRepository<ProjectCollaborators> UserProjectAssignments { get; }

    Task SaveChangesAsync();
}