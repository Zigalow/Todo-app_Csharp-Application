using System.Diagnostics;
using Microsoft.OpenApi.Extensions;
using Todo.Core.Dtos.ProjectCollaborator;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class ProjectCollaboratorMapper
{
    public static ProjectCollaboratorDto ToProjectCollaboratorDto(this ProjectCollaborator projectCollaborator)
    {
        Debug.Assert(projectCollaborator.ApplicationUser.UserName != null, "projectCollaborator.ApplicationUser.UserName != null");
        return new ProjectCollaboratorDto
        {
            UserId = projectCollaborator.UserId,
            Name = projectCollaborator.ApplicationUser.UserName,
            Role = projectCollaborator.Role.GetDisplayName()
        };
    }

    public static ProjectCollaborator ToProjectCollaboratorFromAddDto(
        this AddProjectCollaboratorDto addProjectCollaboratorDto, int projectId)
    {
        return new ProjectCollaborator
        {
            UserId = addProjectCollaboratorDto.UserId,
            ProjectId = projectId,
            Role = addProjectCollaboratorDto.Role
        };
    }

    public static void UpdateProjectCollaboratorFromUpdateDto(this ProjectCollaborator projectCollaborator,
        UpdateProjectCollaboratorDto updateProjectCollaboratorDto)
    {
        projectCollaborator.Role = updateProjectCollaboratorDto.Role;
    }

    public static IEnumerable<ProjectCollaboratorDto> ToListedProjectCollaboratorDtos(
        this IEnumerable<ProjectCollaborator> projectCollaborators)
    {
        return projectCollaborators.Select(projectCollaborator => projectCollaborator.ToProjectCollaboratorDto())
            .ToList();
    }
}