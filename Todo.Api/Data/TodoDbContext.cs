using Microsoft.EntityFrameworkCore;
using Todo.Api.Configuration;
using Todo.Core.Entities;
using Label = Microsoft.Data.SqlClient.DataClassification.Label;

namespace Todo.Api.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectCollaborators> Collaborators { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Label> Labels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
    }
}