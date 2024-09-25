namespace Todo.Core.Entities;

public class TodoList
{
    int Id { get; }
    string Name { get; }
    List<TodoItem> Items { get; }
    List<User> Collaborators { get; }
}