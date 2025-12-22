
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects"); // define explicitamente el nombre de la tabla en la base de datos. Si no pusiera esto, EF Core usa convenciones entonces pluralizaria o usaria el nombre de la clase. ej Project -> Projects, segun el provider/config.

        builder // ID
            .HasKey(b => b.Id); // no hace falta el IsRequired ya que con el HasKey una PK nunca vendra vacia

        builder // NAME
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder // DESCRIPTION
            .Property(b => b.Description)
            .HasMaxLength(500);

        builder // CREATEDAT
            .Property(b => b.CreatedAt)
            .IsRequired();
    }
}