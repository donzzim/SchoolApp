using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infra.Data.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.PasswordHash)
            .IsRequired();
        builder.Property(x => x.PasswordSalt)
            .IsRequired();
        builder.Property(x => x.Profile)
            .IsRequired()
            .HasMaxLength(50);
    }
}
