using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Domain.Interfaces;
using School.Infra.Data.Context;
using School.Infra.Data.Repositories;

namespace School.Infra.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();

        return services;
    }
}
