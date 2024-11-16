using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

public class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
{
    private readonly TodoDbContext _dbContext;

    public TodoItemRepository(TodoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<IEnumerable<TodoItem>> GetAllAsync(string userId)
    {
        return await _dbContext.TodoItems
            .Include(i => i.Labels)
            .Include(i => i.TodoList)
            .Where(i => i.TodoList.Project.AdminId == userId)
            .ToListAsync();
    }

    public async Task<List<TodoItem>?> GetAllTodoItemsForTodoList(int todoListId)
    {
        var todoItemsFromList = await _dbContext.TodoLists
            .Where(tl => tl.Id == todoListId)
            .Include(tl => tl.Items)
            .ThenInclude(i => i.Labels)
            .Select(tl => tl.Items)
            .FirstOrDefaultAsync();

        return todoItemsFromList?.ToList();
    }

    public async Task<List<TodoItem>?> GetAllTodoItemsForProject(int projectId)
    {
        var todoItemsFromProject = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .ThenInclude(i => i.Labels)
            .SelectMany(p => p.TodoLists)
            .Select(tl => new { tl.Items })
            .FirstOrDefaultAsync();

        return todoItemsFromProject?.Items.ToList();
    }

    public override async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _dbContext.TodoItems
            .Include(i => i.Labels)
            .Include(i => i.TodoList)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Result<TodoItem>> AttachLabelToItem(int todoItemId, int labelId)
    {
        var todoItem = await GetByIdAsync(todoItemId);

        if (todoItem == null)
        {
            return Result<TodoItem>.Failure("Todo item not found");
        }

        var label = await _dbContext.Labels.FindAsync(labelId);

        if (label == null)
        {
            return Result<TodoItem>.Failure("Label not found");
        }

        if (label.ProjectId != todoItem.TodoList.ProjectId)
        {
            return Result<TodoItem>.Failure("Label not from the same project as the todo item");
        }

        if (todoItem.Labels.Contains(label))
        {
            return Result<TodoItem>.Failure("Label already attached to the todo item");
        }

        todoItem.Labels.Add(label);

        return Result<TodoItem>.Success(todoItem);
    }

    public async Task<Result<TodoItem>> DetachLabelFromItem(int todoItemId, int labelId)
    {
        var todoItem = await GetByIdAsync(todoItemId);

        if (todoItem == null)
        {
            return Result<TodoItem>.Failure("Todo item not found");
        }

        var label = await _dbContext.Labels.FindAsync(labelId);

        if (label == null)
        {
            return Result<TodoItem>.Failure("Label not found");
        }

        if (label.ProjectId != todoItem.TodoList.ProjectId)
        {
            return Result<TodoItem>.Failure("Label not from the same project as the todo item");
        }

        if (!todoItem.Labels.Contains(label))
        {
            return Result<TodoItem>.Failure("Label not attached to the todo item");
        }

        todoItem.Labels.Remove(label);

        return Result<TodoItem>.Success(todoItem);
    }
}