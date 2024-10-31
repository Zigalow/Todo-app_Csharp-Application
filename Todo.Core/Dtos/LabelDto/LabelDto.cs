namespace Todo.Core.Dtos.LabelDto;

public class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string Color { get; set; } = "000000";
    public int TodoCount { get; set; }
}