using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.ProjectDtos;

public class CreateProjectDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
}