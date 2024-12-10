using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.LabelDtos;

public class UpdateLabelDto
{
    public string? Name { get; set; } = null;

    [StringLength(7)]
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
        ErrorMessage = "Color must be a valid hex color code. Did you remember to include the '#'?")]

    public string? Color { get; set; } = null;
}