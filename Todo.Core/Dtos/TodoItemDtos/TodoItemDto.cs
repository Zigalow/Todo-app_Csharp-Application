namespace Todo.Core.Dtos.TodoItemDtos;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "VeryLow";
    public bool IsDone { get; set; } = false;
    public int TodoListId { get; set; }
    public List<String> Labels { get; set; } = new();
}