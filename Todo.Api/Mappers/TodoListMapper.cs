using Todo.Api.Dtos.TodoListDtos;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class TodoListMapper
{
    public static TodoListDto ToTodoListDto(this TodoList todoList)
    {
        return new TodoListDto
        {
            Id = todoList.Id,
            Name = todoList.Name,
            ProjectId = todoList.ProjectId,
            TodoCount = todoList.Items.Count
        };
    }

    public static TodoList ToTodoListFromCreateDto(this CreateTodoListDto createTodoListDto, int projectId)
    {
        return new TodoList
        {
            ProjectId = projectId,
            Name = createTodoListDto.Name
        };
    }

    public static IEnumerable<TodoListDto> ToListedTodoListDtos(this IEnumerable<TodoList> todoLists)
    {
        return todoLists.Select(todoList => todoList.ToTodoListDto());
    }
}