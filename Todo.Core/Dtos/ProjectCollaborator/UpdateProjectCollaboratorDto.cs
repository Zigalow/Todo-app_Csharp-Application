using System.ComponentModel.DataAnnotations;
using Todo.Core.Entities;

namespace Todo.Core.Dtos.ProjectCollaborator;

public class UpdateProjectCollaboratorDto
{
    [Required]
    [Range(1, 3)]
    public ProjectRole Role { get; set; }
}