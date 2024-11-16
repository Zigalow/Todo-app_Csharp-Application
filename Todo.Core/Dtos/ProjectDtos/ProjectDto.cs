using Todo.Core.Dtos.TodoListDtos;

namespace Todo.Core.Dtos.ProjectDtos;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? AdminId { get; set; } = null!;
    public string? AdminName { get; set; } = null!;

    public int TodoListsCount { get; set; }

    public int TodoItemsCount { get; set; }
    
    public List<TodoListDto> TodoLists { get; set; } = new(); 
}