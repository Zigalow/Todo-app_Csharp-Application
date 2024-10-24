using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.TodoListDtos;

public class UpdateTodoListDto
{

    [StringLength(100)]
    public string? Name { get; set; } = null;
}