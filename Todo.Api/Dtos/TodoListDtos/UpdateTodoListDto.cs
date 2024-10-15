using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.TodoListDtos;

public class UpdateTodoListDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
}