using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infra.Data.EntitiesConfiguration;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId)
            .IsRequired();
        builder.Property(x => x.SchoolClassId)
            .IsRequired();
        builder.Property(x => x.EnrollmentDate)
            .IsRequired();
        builder.Property(x => x.ExpirationDate)
            .IsRequired();
        builder.Property(x => x.IsActive)
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.SchoolClass)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.SchoolClassId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
