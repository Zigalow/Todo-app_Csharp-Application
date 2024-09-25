namespace Todo.Core.Entities;

public static class PermissionManager
{
    [Flags]
    public enum PermissionType
    {
        None = 0,
        ViewTodos = 1 << 0,
        CreateTodos = 1 << 1,
        UpdateTodos = 1 << 2,
        DeleteTodos = 1 << 3,
        ManageProjects = 1 << 4,
        AssignProjectMembers = 1 << 5,
        ManageTodoLists = 1 << 6,
        ShareTodoLists = 1 << 7,
        ManageLabels = 1 << 8,
        ViewUsers = 1 << 9,
        ManageUsers = 1 << 10,
        ManageRoles = 1 << 11,
        ManageAdmin = 1 << 12,
        All = ~0
    }

    public static bool HasPermission(PermissionType permissions, PermissionType permission)
    {
        return (permissions & permission) == permission;
    }

    public static void GrantPermission(ref PermissionType permissions, PermissionType permission)
    {
        permissions |= permission;
    }

    public static void RevokePermission(ref PermissionType permissions, PermissionType permission)
    {
        permissions &= ~permission;
    }
}