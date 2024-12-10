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
    public ApplicationUser Owner { get; set; } = null!;

    [Required]
    public ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();

    public ICollection<ProjectCollaborator> Collaborators { get; set; } = new List<ProjectCollaborator>();
}