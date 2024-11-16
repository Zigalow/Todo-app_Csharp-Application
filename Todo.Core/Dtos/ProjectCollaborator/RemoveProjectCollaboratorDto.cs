using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.ProjectCollaborator;

public class RemoveProjectCollaboratorDto
{
    [Required]
    public string UserId { get; set; } = null!;
}