using System.ComponentModel.DataAnnotations;
using Todo.Core.Entities;

namespace Todo.Api.Dtos.TodoItemDtos;

public class UpdateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }

    [Range(0, 4)]
    public Priority Priority { get; set; } = Priority.VeryLow;

    public bool IsDone { get; set; } = false;
}