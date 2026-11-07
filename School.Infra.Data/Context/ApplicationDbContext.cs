using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {

    }

    public DbSet<Course> Course { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Grade> Grade { get; set; }
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}