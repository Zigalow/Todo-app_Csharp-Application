using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo.Core.Entities;

[PrimaryKey(nameof(UserId), nameof(ProjectId))]
public class ProjectCollaborators
{
    public int UserId { get; set; }

    public int ProjectId { get; set; }

    [Required]
    public int RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;
}