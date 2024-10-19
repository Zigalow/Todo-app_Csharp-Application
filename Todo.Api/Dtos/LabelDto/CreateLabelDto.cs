using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.LabelDto;

public class CreateLabelDto
{
    public string Name { get; set; } = string.Empty;

    [StringLength(7)]
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
        ErrorMessage = "Color must be a valid hex color code. Did you remember to include the '#'?")]
    public string? Color { get; set; } = null;
}