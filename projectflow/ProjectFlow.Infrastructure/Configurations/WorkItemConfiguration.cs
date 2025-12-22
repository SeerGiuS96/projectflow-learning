using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Infrastructure.Configurations;

public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("WorkItems"); // define explicitamente el nombre de la tabla en la base de datos.

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ProjectId)
           .IsRequired();

        builder.Property(b => b.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(b => b.Description)
               .HasMaxLength(1000);

        builder.Property(b => b.Status)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(b => b.Priority)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.Property(b => b.CompletedAt);
    }
}