namespace Todo.Core.Entities;

public class UserProjectAssignment
{
    public int UserId { get; }
    public int ProjectId { get; }
    public Role Role { get; }
}