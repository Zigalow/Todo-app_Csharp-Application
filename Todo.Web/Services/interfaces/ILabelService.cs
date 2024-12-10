using Todo.Core.Dtos.LabelDtos;
using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Web.Services.interfaces;

public interface ILabelService
{
    Task<List<LabelDto>> GetAllLabelsAsync();
    Task<List<LabelDto?>> GetAllLabelsForProject(int projectId);
    Task<LabelDto?> GetLabelByIdAsync(int id);
    Task<bool> CreateLabelAsync(int projectId, CreateLabelDto labelDto);
    Task<bool> UpdateLabelAsync(int id, UpdateLabelDto label);
    Task<bool> DeleteLabelAsync(int id);
}