using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Core.Entities;

public class TodoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Required]
    public Priority Priority { get; set; } = Priority.VeryLow;

    public bool IsDone { get; set; } = false;

    [Required]
    public int TodoListId { get; set; }

    [ForeignKey(nameof(TodoListId))]
    public TodoList TodoList { get; set; } = null!;
    public ICollection<Label> Labels { get; set; } = new List<Label>();
    // public ICollection<ApplicationUser> Assignees { get; set; } = new List<ApplicationUser>();
}