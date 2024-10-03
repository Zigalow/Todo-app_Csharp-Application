using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Api.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(b => b.Permissions)
            .HasConversion(
                v => string.Join(',', v.Select(p => p.ToString())),
                v => new HashSet<PermissionType>(v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Enum.Parse<PermissionType>)));
    }
}