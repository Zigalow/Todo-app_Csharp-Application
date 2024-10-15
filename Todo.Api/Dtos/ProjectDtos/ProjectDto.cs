namespace Todo.Api.Dtos.ProjectDtos;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public string? AdminName { get; set; } = null!;
}