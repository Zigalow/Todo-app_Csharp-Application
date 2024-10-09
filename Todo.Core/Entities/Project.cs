using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Core.Entities;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string AdminId { get; set; } = null!;

    [ForeignKey(nameof(AdminId))]
    public ApplicationUser Admin { get; set; } = null!;

    [Required]
    public ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();

    public ICollection<ProjectCollaborators> Collaborators { get; set; } = new List<ProjectCollaborators>();
}