namespace Todo.Api.Dtos.TodoListDtos;

public class TodoListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    
    public int TodoCount { get; set; }
}