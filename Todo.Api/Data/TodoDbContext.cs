using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities;

namespace Todo.Api.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    public TodoItem Type { get; set; }

}