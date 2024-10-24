using Todo.Api.Dtos.TodoListDtos;

namespace Todo.Api.Dtos.ProjectDtos;

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