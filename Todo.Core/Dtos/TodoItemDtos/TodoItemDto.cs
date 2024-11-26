using System.ComponentModel.DataAnnotations;
using Todo.Core.Dtos.LabelDtos;
using Todo.Core.Entities;

namespace Todo.Core.Dtos.TodoItemDtos;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    [Range(0, 4)]
    public Priority Priority { get; set; }
    public bool IsDone { get; set; } = false;
    public int TodoListId { get; set; }
    public List<LabelDto> Labels { get; set; } = new();
}