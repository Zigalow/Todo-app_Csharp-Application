using Todo.Core.Dtos.LabelDto;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class LabelMapper
{
    public static LabelDto ToLabelDto(this Label label)
    {
        return new LabelDto
        {
            Id = label.Id,
            Name = label.Name,
            ProjectId = label.ProjectId,
            Color = label.Color,
            TodoCount = label.TodoItems?.Count ?? 0
        };
    }

    public static Label ToLabelFromCreateDto(this CreateLabelDto createLabelDto, int projectId)
    {
        return new Label
        {
            Name = createLabelDto.Name,
            ProjectId = projectId,
            Color = createLabelDto.Color ?? "#000000",
        };
    }

    public static void UpdateLabelFromUpdateDto(this Label label, UpdateLabelDto updateLabelDto)
    {
        if (updateLabelDto.Name is not null)
            label.Name = updateLabelDto.Name;
        if (updateLabelDto.Color is not null)
            label.Color = updateLabelDto.Color;
    }

    public static IEnumerable<LabelDto> ToListedLabelDtos(this IEnumerable<Label> labels)
    {
        return labels.Select(label => label.ToLabelDto());
    }
}