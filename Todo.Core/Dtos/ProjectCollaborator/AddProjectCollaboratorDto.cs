using System.ComponentModel.DataAnnotations;
using Todo.Core.Entities;

namespace Todo.Core.Dtos.ProjectCollaborator;

public class AddProjectCollaboratorDto
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [Range(1, 4)]
    public ProjectRole Role { get; set; }
}