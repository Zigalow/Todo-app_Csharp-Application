using Microsoft.OpenApi.Extensions;
using Todo.Api.Dtos.TodoItemDtos;
using Todo.Core.Entities;

namespace Todo.Api.Mappers;

public static class TodoItemMapper
{
    public static TodoItemDto ToTodoItemDto(this TodoItem todoItem)
    {
        return new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            DueDate = todoItem.DueDate,
            Priority = todoItem.Priority.GetDisplayName(),
            IsDone = todoItem.IsDone,
            TodoListId = todoItem.TodoListId,
            Labels = todoItem.Labels.ToList()
        };
    }

    public static TodoItem ToTodoItemFromCreateDto(this CreateTodoItemDto todoItemDto, int todoListId)
    {
        return new TodoItem()
        {
            Title = todoItemDto.Title,
            Description = todoItemDto.Description,
            DueDate = todoItemDto.DueDate,
            Priority = todoItemDto.Priority,
            TodoListId = todoListId,
        };
    }
}