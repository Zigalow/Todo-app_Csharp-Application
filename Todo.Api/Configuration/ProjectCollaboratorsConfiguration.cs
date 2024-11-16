using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Api.Configuration;

public class ProjectCollaboratorsConfiguration : IEntityTypeConfiguration<ProjectCollaborator>
{
    public void Configure(EntityTypeBuilder<ProjectCollaborator> builder)
    {
        builder.Property(pc => pc.Role)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ProjectRole>(v));
    }
}