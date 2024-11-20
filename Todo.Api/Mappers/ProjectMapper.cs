using Todo.Core.Dtos.ProjectDtos;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class ProjectMapper
{
    public static ProjectDto ToProjectDto(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            AdminId = project.AdminId,
            AdminName = project.Owner.UserName,
            TodoListsCount = project.TodoLists.Count,
            TodoItemsCount = project.TodoLists.Sum(tl => tl.Items.Count),
            TodoLists = project.TodoLists.ToListedTodoListDtos().ToList()
        };
    }

    public static SharedProjectDto ToSharedProjectDto(this Project project, ProjectRole role)
    {
        return new SharedProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            AdminId = project.AdminId,
            AdminName = project.Owner.UserName,
            TodoListsCount = project.TodoLists.Count,
            TodoItemsCount = project.TodoLists.Sum(tl => tl.Items.Count),
            TodoLists = project.TodoLists.ToListedTodoListDtos().ToList(),
            RoleInProject = role
        };
    }

    public static Project ToProjectFromCreateDto(this CreateProjectDto createProjectDto, string userId)
    {
        return new Project
        {
            Name = createProjectDto.Name,
            AdminId = userId,
        };
    }

    public static void UpdateProjectFromUpdateDto(this Project project, UpdateProjectDto updateProjectDto)
    {
        if (updateProjectDto.Name is not null)
            project.Name = updateProjectDto.Name;
    }

    public static IEnumerable<ProjectDto> ToListedProjectDtos(this IEnumerable<Project> projects)
    {
        return projects.Select(project => project.ToProjectDto());
    }

    public static IEnumerable<SharedProjectDto> ToListedSharedProjectDtos(
        this Dictionary<Project, ProjectRole> projectsWithRoles)
    {
        return projectsWithRoles.Select(p => p.Key.ToSharedProjectDto(p.Value));
    }
}