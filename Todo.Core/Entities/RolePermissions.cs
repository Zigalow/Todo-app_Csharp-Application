namespace Todo.Core.Entities;

public static class RolePermissions
{
    private static readonly Dictionary<ProjectRole, HashSet<string>> _permissions = new()
    {
        // Admin has all permissions
        [ProjectRole.Admin] =
        [
            Permissions.ManageProject,
            Permissions.ManageProjectCollaborators,
            Permissions.ManageRoles,
            Permissions.Viewer,
            Permissions.ManageTodoLists,
            Permissions.ManageTodoItems,
            Permissions.ManageLabels
        ],

        // Owner has all permissions except managing admins and modifying project
        [ProjectRole.ProjectOwner] = new HashSet<string>
        {
            Permissions.ManageProjectCollaborators,
            Permissions.ManageRoles,
            Permissions.Viewer,
            Permissions.ManageTodoLists,
            Permissions.ManageTodoItems,
            Permissions.ManageLabels
        },

        // Member has todo management permissions
        [ProjectRole.ProjectMember] = new HashSet<string>
        {
            Permissions.Viewer,
            Permissions.ManageTodoLists,
            Permissions.ManageTodoItems,
            Permissions.ManageLabels,
        },

        // Viewer has only view permissions
        [ProjectRole.Viewer] = new HashSet<string>
        {
            Permissions.Viewer
        }
    };

    public static bool HasPermission(ProjectRole role, string permission)
    {
        return _permissions[role].Contains(permission);
    }

    public static IEnumerable<string> GetPermissions(ProjectRole role)
    {
        var f = _permissions[ProjectRole.Admin];

        return _permissions[role];
    }

    public static bool CanManageRole(ProjectRole managerRole, ProjectRole targetRole)
    {
        // Admin cannot be managed by anyone
        if (targetRole == ProjectRole.Admin)
        {
            return false;
        }

        // Only Admin and Owner can manage roles
        if (managerRole != ProjectRole.Admin && managerRole != ProjectRole.ProjectOwner)
        {
            return false;
        }

        return true;
    }
}