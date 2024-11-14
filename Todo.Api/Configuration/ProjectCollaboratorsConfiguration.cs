using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Api.Configuration;

public class ProjectCollaboratorsConfiguration : IEntityTypeConfiguration<ProjectCollaborators>
{
    public void Configure(EntityTypeBuilder<ProjectCollaborators> builder)
    {
        builder.Property(pc => pc.Role)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ProjectRole>(v));
    }
}