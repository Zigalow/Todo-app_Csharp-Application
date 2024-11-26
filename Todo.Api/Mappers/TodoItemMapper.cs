using Microsoft.OpenApi.Extensions;
using Todo.Core.Dtos.TodoItemDtos;
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
            Priority = todoItem.Priority,
            IsDone = todoItem.IsDone,
            TodoListId = todoItem.TodoListId,
            Labels = todoItem.Labels.ToListedLabelDtos().ToList()
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

    public static void UpdateTodoItemFromUpdateDto(this TodoItem todoItem, UpdateTodoItemDto updateTodoItemDto)
    {
        if (updateTodoItemDto.Title is not null)
            todoItem.Title = updateTodoItemDto.Title;
        if (updateTodoItemDto.Description is not null)
            todoItem.Description = updateTodoItemDto.Description;
        if (updateTodoItemDto.DueDate is not null)
            todoItem.DueDate = updateTodoItemDto.DueDate;
        if (updateTodoItemDto.Priority is not null)
            todoItem.Priority = updateTodoItemDto.Priority ?? todoItem.Priority;
        if (updateTodoItemDto.IsDone is not null)
            todoItem.IsDone = updateTodoItemDto.IsDone ?? todoItem.IsDone;
    }

    public static IEnumerable<TodoItemDto> ToListedTodoItemsDtos(this IEnumerable<TodoItem> todoItems)
    {
        return todoItems.Select(todoItem => todoItem.ToTodoItemDto());
    }
}