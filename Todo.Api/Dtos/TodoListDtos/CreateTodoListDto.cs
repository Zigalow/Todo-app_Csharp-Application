using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.TodoListDtos;

public class CreateTodoListDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
}