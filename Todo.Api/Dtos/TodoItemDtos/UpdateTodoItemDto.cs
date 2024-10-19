using System.ComponentModel.DataAnnotations;
using Todo.Core.Entities;

namespace Todo.Api.Dtos.TodoItemDtos;

public class UpdateTodoItemDto
{
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public DateTime? DueDate { get; set; } = null;

    [Range(0, 4)]
    public Priority? Priority { get; set; } = null;

    public bool? IsDone { get; set; } = null;
}