namespace Todo.Core.Entities;

public class User
{
    public int Id { get; }
    public int Name { get; }
    public int Email { get; }
    public string PasswordHash { get; }
    public List<Project> Projects { get; }
}