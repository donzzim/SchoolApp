using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infra.Data.EntitiesConfiguration;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.GradeValue)
            .IsRequired();
        builder.Property(x => x.DateGrade)
            .IsRequired();
        builder.Property(x => x.EnrollmentId)
            .IsRequired();

        builder.HasOne(x => x.Enrollment)
            .WithMany(x => x.Grades)
            .HasForeignKey(x => x.EnrollmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
