using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Todo.Core.Entities;

[PrimaryKey(nameof(UserId), nameof(ProjectId))]
public class ProjectCollaborators
{
    public string UserId { get; set; }

    public int ProjectId { get; set; }

    [Required]
    public string RoleId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public ApplicationRole ApplicationRole { get; set; } = null!;
}