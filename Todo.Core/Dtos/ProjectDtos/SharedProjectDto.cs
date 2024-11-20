using Todo.Core.Entities;

namespace Todo.Core.Dtos.ProjectDtos;

public class SharedProjectDto : ProjectDto
{
    public ProjectRole RoleInProject { get; set; }
}