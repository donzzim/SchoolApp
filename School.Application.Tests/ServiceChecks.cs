using School.Application.DTO.Course;
using School.Application.DTO.SchoolClass;
using School.Application.DTO.Grade;
using School.Application.DTO.User;
using School.Application.Services;
using School.Domain.Entities;
using School.Domain.Interfaces;

var courses = new FakeCourseRepository();
var courseService = new CourseService(courses);
var course = await courseService.AddAsync(new CoursePostDto { Name = " Curso ", Description = "Descrição" });
Check(course.Name == " Curso ", "Mapeamento direto sem normalização");
var schoolClasses = new FakeSchoolClassRepository();
var schoolClassService = new SchoolClassService(schoolClasses);
var schoolClass = await schoolClassService.AddAsync(new SchoolClassPostDto { Name = "Turma", Description = "Descrição", CourseId = course.Id });
schoolClasses.Items.Single().Course = courses.Items.Single();
Check((await schoolClassService.GetByIdAsync(schoolClass.Id))!.Course!.Id == course.Id, "Consulta detalhada de turma");
Check((await schoolClassService.GetAllAsync()).Single().CourseId == course.Id, "Listagem de turmas");
var users = new FakeUserRepository();
var userService = new UserService(users);
var user = await userService.AddAsync(new UserPostDto { Name = "Aluno", Email = "a@example.com", Password = "senha-teste" });
var salt = users.Items.Single().PasswordSalt!.ToArray();
Check(users.Items.Single().PasswordHash!.Length == 32, "Hash gerado");
var updatedUser = await userService.UpdateAsync(user.Id, new UserPutDto { Name = "Novo", Email = "b@example.com", Password = "outra-senha" });
Check(updatedUser!.Id == user.Id && updatedUser.Name == "Novo", "ID de usuário recebido separadamente");
Check(!salt.SequenceEqual(users.Items.Single().PasswordSalt!), "Troca de senha renova salt");
Check(await userService.UpdateAsync(999, new UserPutDto { Name = "Ausente", Email = "x@example.com", Password = "senha-teste" }) is null, "Atualização ausente");
var enrollments = new FakeEnrollmentRepository();
enrollments.Items.Add(new Enrollment { Id = 1, SchoolClassId = schoolClass.Id });
var grades = new FakeGradeRepository();
var gradeService = new GradeService(grades, enrollments, schoolClasses);
var grade = await gradeService.AddAsync(new GradePostDto { EnrollmentId = 1, GradeValue = 80 });
Check(grade.EnrollmentId == 1 && grade.GradeValue == 80 && grade.IsApproved && grade.DateGrade != default, "Mapeamento de criação da nota");
var changed = await gradeService.UpdateAsync(new GradePutDto { Id = grade.Id, EnrollmentId = 1, GradeValue = 90 });
Check(changed!.GradeValue == 90 && grades.Items.Single().CourseId == course.Id, "Atualização resolve o curso pela SchoolClass");
Check(await courseService.GetByIdAsync(999) is null && await schoolClassService.GetByIdAsync(999) is null &&
      await userService.GetByIdAsync(999) is null && await gradeService.GetByIdAsync(999) is null, "Consultas ausentes");
Check(await courseService.DeleteAsync(999) is null && await schoolClassService.DeleteAsync(999) is null &&
      await userService.DeleteAsync(999) is null && await gradeService.DeleteAsync(999) is null, "Exclusões ausentes");
await courseService.DeleteAsync(course.Id);
Check((await courseService.GetAllAsync()).Count == 1, "Serviço devolve registros fornecidos pelo repositório");
Console.WriteLine("Todas as verificações de serviços passaram.");

static void Check(bool value, string message)
{
    if (!value) throw new Exception(message);
    Console.WriteLine($"OK: {message}");
}

sealed class FakeCourseRepository : ICourseRepository
{
    public List<Course> Items { get; } = [];
    public Task<Course?> GetByIdAsync(int? id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Course>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Course> AddAsync(Course entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Course?> UpdateAsync(Course entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<Course?>(null); Items[index] = entity; return Task.FromResult<Course?>(entity); }
    public Task<Course?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeSchoolClassRepository : ISchoolClassRepository
{
    public List<SchoolClass> Items { get; } = [];
    public Task<SchoolClass?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<SchoolClass>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<SchoolClass> AddAsync(SchoolClass entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<SchoolClass?> UpdateAsync(SchoolClass entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<SchoolClass?>(null); Items[index] = entity; return Task.FromResult<SchoolClass?>(entity); }
    public Task<SchoolClass?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeUserRepository : IUserRepository
{
    public List<User> Items { get; } = [];
    public Task<User?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<User>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<User> AddAsync(User entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<User?> UpdateAsync(User entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<User?>(null); Items[index] = entity; return Task.FromResult<User?>(entity); }
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
    public Task<Grade?> UpdateAsync(Grade entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<Grade?>(null); Items[index] = entity; return Task.FromResult<Grade?>(entity); }
    public Task<Grade?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}