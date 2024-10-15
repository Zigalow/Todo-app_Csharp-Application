using Todo.Api.Dtos.TodoListDtos;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class TodoListMapper
{
    public static TodoList ToTodoListFromCreateDto(this CreateTodoListDto createTodoListDto, int projectId)
    {
        return new TodoList
        {
            ProjectId = projectId,
            Name = createTodoListDto.Name
        };
    }
}