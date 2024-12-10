using System.ComponentModel;

namespace Todo.Core.Entities;

public enum ProjectRole
{
    [Description("Admin - Full control over projects and team members")]
    Admin,

    [Description("Project Owner - Can manage most of the project and team members")]
    ProjectOwner,

    [Description("Project Member - Can manage todo lists, items and labels")]
    ProjectMember,

    [Description("Project Viewer - Can view projects")]
    Viewer
}