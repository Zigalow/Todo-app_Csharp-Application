using Todo.Core.Dtos.TodoListDtos;
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
            TodoCount = todoList.Items.Count,
            TodoItems = todoList.Items.ToListedTodoItemsDtos().ToList()
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

    public static void UpdateTodoListFromUpdateDto(this TodoList todoList, UpdateTodoListDto updateTodoListDto)
    {
        if (updateTodoListDto.Name is not null)
            todoList.Name = updateTodoListDto.Name;
    }

    public static IEnumerable<TodoListDto> ToListedTodoListDtos(this IEnumerable<TodoList> todoLists)
    {
        return todoLists.Select(todoList => todoList.ToTodoListDto());
    }
}