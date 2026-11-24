using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Interfaces;
using School.Application.Services;
using School.Domain.Account;
using School.Domain.Interfaces;
using School.Infra.Data.Context;
using School.Infra.Data.Identity;
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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["Jwt:Issuer"] /*quem emitiu o token*/,
                ValidAudience = configuration["Jwt:Audience"] /*quem consome o token*/,
                IssuerSigningKey =
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISchoolClassService, SchoolClassService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IAuthenticate, AuthenticateService>();

        return services;
    }
}
