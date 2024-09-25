namespace Todo.Core.Entities;

public class TodoItem
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime? DueDate { get; }
    public Priority Priority { get; }
    public bool IsDone { get; }
    public int TodoListId { get; }
    public TodoList TodoList { get; }
    public List<Label> Labels { get; }
    public List<User> Assignees { get; }
}