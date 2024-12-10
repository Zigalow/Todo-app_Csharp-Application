using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Core.Dtos.TodoListDtos;

public class TodoListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TodoCount { get; set; }
    public int ProjectId { get; set; }
    
    public List<TodoItemDto> TodoItems { get; set; } = new();
}