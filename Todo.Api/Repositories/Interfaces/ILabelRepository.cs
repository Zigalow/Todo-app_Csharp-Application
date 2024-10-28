using Todo.Core.Entities;

namespace Todo.Api.Repositories.Interfaces;

public interface ILabelRepository : IRepository<Label>
{
    Task<List<Label>?> GetAllLabelsForProject(int projectId);

    Task<bool> ExistsAsync(Label createLabelDto);
}