using Todo.Api.Dtos.ProjectDtos;
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
            AdminName = project.Owner != null ? project.Owner.UserName : "No user" 
        };
    }

    public static Project ToProjectFromCreateDto(this CreateProjectDto createProjectDto)
    {
        return new Project
        {
            Name = createProjectDto.Name,
            AdminId = "12345" // Hardcoded for now so they are all linked to the same aspnet user
        };
    }
}