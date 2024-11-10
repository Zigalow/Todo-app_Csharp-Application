using Todo.Core.Dtos.LabelDto;
using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Web.Services.interfaces;

public interface ILabelService
{
    Task<List<LabelDto>> GetAllLabelsAsync();
    Task<TodoItemDto?> GetAllLabelsForProject(int projectId);
    Task<LabelDto?> GetLabelByIdAsync(int id);
    Task<bool> CreateLabelAsync(CreateLabelDto labelDto);
    Task<bool> UpdateLabelAsync(int id, UpdateLabelDto label);
    Task<bool> DeleteLabelAsync(int id);
}