using System.ComponentModel;

namespace Todo.Core.Entities;

public static class Permissions
{
    // Project permissions
    [Description("Can view project details and members")]
    public const string Viewer = "Viewer";

    [Description("Can add/remove project members")]
    public const string ManageCollaborators = "ManageCollaborators";

    [Description("Can change roles of other members")]
    public const string ManageRoles = "ManageRoles";

    [Description("Can update and delete project")]
    public const string ManageProject = "ManageProject";

    // TodoList permissions
    [Description("Can create, update and delete todoLists")]
    public const string ManageTodoLists = "ManageTodoLists";

    // TodoItem permissions
    [Description("Can create, update and delete todoItems")]
    public const string ManageTodoItems = "ManageTodoItems";

    // Label permissions
    [Description("Can create, update and delete labels")]
    public const string ManageLabels = "ManageLabels";
}