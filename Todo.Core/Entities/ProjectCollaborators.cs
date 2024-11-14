using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo.Core.Entities;

[PrimaryKey(nameof(UserId), nameof(ProjectId))]
public class ProjectCollaborators
{
    public string UserId { get; set; }

    public int ProjectId { get; set; }

    public ProjectRole Role { get; set; }

    [ForeignKey(nameof(UserId))]

    public ApplicationUser ApplicationUser { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;
}