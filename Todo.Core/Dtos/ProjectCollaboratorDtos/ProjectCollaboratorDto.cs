using Todo.Core.Entities;

namespace Todo.Core.Dtos.ProjectCollaboratorDtos;

public class ProjectCollaboratorDto
{
    public string Name { get; set; } = null!;
    
    public ProjectRole Role { get; set; }
    
    public string UserId { get; set; } = null!;
}