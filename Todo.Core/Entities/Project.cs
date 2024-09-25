namespace Todo.Core.Entities;

public class Project
{
    public int Id { get; }
    public int Name { get; }
    public User Owner { get; }
}