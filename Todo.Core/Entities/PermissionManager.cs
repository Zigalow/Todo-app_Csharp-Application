namespace Todo.Core.Entities;

public static class PermissionManager
{
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