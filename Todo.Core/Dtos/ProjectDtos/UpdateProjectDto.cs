using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.ProjectDtos;

public class UpdateProjectDto
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; } = null;
}