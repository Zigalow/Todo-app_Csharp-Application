using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Configuration;
using Todo.Core.Entities;

namespace Todo.Api.Data;

public class TodoDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public TodoDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<ProjectCollaborator> ProjectCollaborators { get; set; }
    public DbSet<Label> Labels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectCollaboratorsConfiguration());
        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
    }
}