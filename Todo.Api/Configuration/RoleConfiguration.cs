using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Api.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        var permissionTypeComparer = new ValueComparer<HashSet<PermissionType>>(
            (c1, c2) => c1.SetEquals(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => new HashSet<PermissionType>(c));

        builder.Property(b => b.Permissions)
            .HasConversion(
                v => string.Join(',', v.Select(p => p.ToString())),
                v => new HashSet<PermissionType>(v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Enum.Parse<PermissionType>)))
            .Metadata.SetValueComparer(permissionTypeComparer);
    }
}