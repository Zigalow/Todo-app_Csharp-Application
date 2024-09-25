namespace Todo.Core.Entities;

public class Role
{
    int Id { get; }
    string Name { get; }
    HashSet<PermissionManager.PermissionType> Permissions { get; }
}