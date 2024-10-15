using Todo.Core.Entities;

namespace Todo.Api.Dtos.TodoItemDtos;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.VeryLow;
    public bool IsDone { get; set; } = false;
    public int TodoListId { get; set; }
    public List<Label> Labels { get; set; } = new();
}