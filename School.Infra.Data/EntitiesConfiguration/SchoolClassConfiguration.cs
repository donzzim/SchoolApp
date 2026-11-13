using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infra.Data.EntitiesConfiguration;

public class SchoolClassConfiguration : IEntityTypeConfiguration<SchoolClass>
{
    public void Configure(EntityTypeBuilder<SchoolClass> builder)
    {
        builder.ToTable("SchoolClass");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(x => x.CourseId)
            .IsRequired();
        builder.HasOne(x => x.Course)
            .WithMany(x => x.SchoolClasses)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
