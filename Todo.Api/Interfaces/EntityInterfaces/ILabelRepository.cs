using Todo.Core.Entities;

namespace Todo.Api.Interfaces.EntityInterfaces;

public interface ILabelRepository : IRepository<Label>
{
    Task<List<Label>?> GetAllLabelsForProject(int projectId);

    Task<bool> ExistsAsync(Label createLabelDto);
}