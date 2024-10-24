using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}