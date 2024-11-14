using Todo.Api.Repositories.Interfaces;
using Todo.Core.Entities;

namespace Todo.Api.Interfaces;

public interface IUnitOfWork
{
    IRepository<ApplicationUser> Users { get; }
    ITodoItemRepository TodoItems { get; }
    ITodoListRepository TodoLists { get; }
    IProjectRepository Projects { get; }
    ILabelRepository Labels { get; }
    IRepository<ProjectCollaborators> ProjectCollaborators { get; }

    Task SaveChangesAsync();
}