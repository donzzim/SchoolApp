using System.ComponentModel.DataAnnotations;
using School.Application.DTO.Course;
using School.Application.DTO.Class;
using School.Application.DTO.Enrollment;
using School.Application.DTO.Grade;
using School.Application.DTO.User;
using School.Application.Services;
using School.Domain.Entities;
using School.Domain.Interfaces;

var courses = new FakeCourseRepository();
var service = new CourseService(courses);
var course = await service.AddAsync(new CoursePostDto { Name = " Curso ", Description = " Descrição " });
Check(course.Name == "Curso", "Normalização de texto");
await Throws<ValidationException>(() => service.AddAsync(new CoursePostDto { Name = " " }));
await Throws<ArgumentOutOfRangeException>(() => service.DeleteAsync(0));
Check(await service.UpdateAsync(new CoursePutDto { Id = 999, Name = "X", Description = "Y" }) is null, "Atualização ausente");
await service.UpdateAsync(new CoursePutDto { Id = course.Id, Name = "Novo", Description = "Texto" });
Check((await service.GetByIdAsync(course.Id))!.Name == "Novo", "Atualização persistida");
Check(await service.DeleteAsync(course.Id) is not null && courses.Items.Single().Deleted, "Exclusão efetiva");
Check(await service.GetByIdAsync(course.Id) is null && (await service.GetAllAsync()).Count == 0, "Exclusão oculta nas consultas");
Check(await service.DeleteAsync(course.Id) is null, "Exclusão repetida");
var activeCourse = await service.AddAsync(new CoursePostDto { Name = "Ativo", Description = "Curso ativo" });
var classes = new FakeClassRepository();
var classService = new ClassService(classes, courses);
await Throws<ValidationException>(() => classService.AddAsync(new ClassPostDto { Name = "T", Description = "D", CourseId = course.Id }));
var schoolClass = await classService.AddAsync(new ClassPostDto { Name = "T", Description = "D", CourseId = activeCourse.Id });
var users = new FakeUserRepository();
var userService = new UserService(users);
var user = await userService.AddAsync(new UserPostDto { Name = "Aluno", Email = "aluno@example.com", Profile = "Student", Password = "senha-teste" });
var hash = users.Items.Single().PasswordHash!.ToArray();
var salt = users.Items.Single().PasswordSalt!.ToArray();
Check(hash.Length == 32 && salt.Length == 16, "Hash e salt gerados");
Check(typeof(UserGetDto).GetProperties().All(p => !p.Name.Contains("Password")), "DTO sem credenciais");
await userService.UpdateAsync(new UserPutDto { Id = user.Id, Name = "Aluno", Email = "aluno@example.com", Profile = "Student" });
Check(hash.SequenceEqual(users.Items.Single().PasswordHash!), "Senha preservada em atualização parcial da credencial");
await userService.UpdateAsync(new UserPutDto { Id = user.Id, Name = "Aluno", Email = "aluno@example.com", Profile = "Student", Password = "outra-senha" });
Check(!salt.SequenceEqual(users.Items.Single().PasswordSalt!), "Troca de senha renova salt");
var enrollments = new FakeEnrollmentRepository();
var enrollmentService = new EnrollmentService(enrollments, users, classes);
var now = DateTime.UtcNow;
await Throws<ValidationException>(() => enrollmentService.AddAsync(new EnrollmentPostDto { UserId = user.Id, ClassId = schoolClass.Id, EnrollmentDate = now, ExpirationDate = now.AddDays(-1) }));
var enrollment = await enrollmentService.AddAsync(new EnrollmentPostDto { UserId = user.Id, ClassId = schoolClass.Id, EnrollmentDate = now, ExpirationDate = now.AddDays(30), IsActive = true });
var grades = new FakeGradeRepository();
var gradeService = new GradeService(grades, courses, enrollments, classes);
var other = await service.AddAsync(new CoursePostDto { Name = "Outro", Description = "Outro curso" });
await Throws<ValidationException>(() => gradeService.AddAsync(new GradePostDto { CourseId = other.Id, EnrollmentId = enrollment.Id, GradeValue = 8, DateGrade = now }));
Check((await gradeService.AddAsync(new GradePostDto { CourseId = activeCourse.Id, EnrollmentId = enrollment.Id, GradeValue = 8, DateGrade = now })).GradeValue == 8, "Nota vinculada ao curso correto");
await classService.DeleteAsync(schoolClass.Id);
await enrollmentService.DeleteAsync(enrollment.Id);
await gradeService.DeleteAsync(grades.Items.Single().Id);
await userService.DeleteAsync(user.Id);
Check((await classService.GetAllAsync()).Count == 0 && (await enrollmentService.GetAllAsync()).Count == 0 && (await gradeService.GetAllAsync()).Count == 0 && (await userService.GetAllAsync()).Count == 0, "Exclusão lógica em todos os serviços");
Console.WriteLine("Todas as verificações de serviços passaram.");

static void Check(bool value, string message)
{
    if (!value) throw new Exception(message);
    Console.WriteLine($"OK: {message}");
}
static async Task Throws<T>(Func<Task> action) where T : Exception
{
    try { await action(); } catch (T) { return; }
    throw new Exception($"Esperada exceção {typeof(T).Name}");
}

sealed class FakeCourseRepository : ICourseRepository
{
    public List<Course> Items { get; } = [];
    public Task<Course?> GetByIdAsync(int? id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Course>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Course> AddAsync(Course entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Course> UpdateAsync(Course entity) => Task.FromResult(entity);
    public Task<Course?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeClassRepository : IClassRepository
{
    public List<Class> Items { get; } = [];
    public Task<Class?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Class>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Class> AddAsync(Class entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Class> UpdateAsync(Class entity) => Task.FromResult(entity);
    public Task<Class?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeUserRepository : IUserRepository
{
    public List<User> Items { get; } = [];
    public Task<User?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<User>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<User> AddAsync(User entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<User> UpdateAsync(User entity) => Task.FromResult(entity);
    public Task<User?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeEnrollmentRepository : IEnrollmentRepository
{
    public List<Enrollment> Items { get; } = [];
    public Task<Enrollment?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Enrollment>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Enrollment> AddAsync(Enrollment entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Enrollment> UpdateAsync(Enrollment entity) => Task.FromResult(entity);
    public Task<Enrollment?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeGradeRepository : IGradeRepository
{
    public List<Grade> Items { get; } = [];
    public Task<Grade?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Grade>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Grade> AddAsync(Grade entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Grade> UpdateAsync(Grade entity) => Task.FromResult(entity);
    public Task<Grade?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}
