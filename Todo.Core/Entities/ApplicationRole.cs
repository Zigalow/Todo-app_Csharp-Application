using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public HashSet<PermissionType> Permissions { get; set; } =
        new HashSet<PermissionType>();

    public IEnumerable<Claim> GetClaims()
    {
        var claims = new List<Claim>();
        foreach (var permission in Permissions)
        {
            claims.Add(new Claim("Permission", permission.ToString()));
        }

        return claims;
    }
}