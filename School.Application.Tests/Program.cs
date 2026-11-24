using School.Application.DTO.Course;
using School.Application.DTO.SchoolClass;
using School.Application.DTO.Grade;
using School.Application.DTO.User;
using School.Application.Services;
using School.Application.Exceptions;
using School.Application.DTO.Enrollment;
using School.Domain.Entities;
using School.Domain.Interfaces;

var courses = new FakeCourseRepository();
var courseService = new CourseService(courses);
var course = await courseService.AddAsync(new CoursePostDto { Name = " Curso ", Description = "Descrição" });
Check(course.Name == " Curso ", "Mapeamento direto sem normalização");
var schoolClasses = new FakeSchoolClassRepository();
var schoolClassService = new SchoolClassService(schoolClasses, courses);
var schoolClass = await schoolClassService.AddAsync(new SchoolClassPostDto { Name = "Turma", Description = "Descrição", CourseId = course.Id });
schoolClasses.Items.Single().Course = courses.Items.Single();
Check((await schoolClassService.GetByIdAsync(schoolClass.Id))!.Course!.Id == course.Id, "Consulta detalhada de turma");
Check((await schoolClassService.GetAllAsync()).Single().CourseId == course.Id, "Listagem de turmas");
var users = new FakeUserRepository();
var userService = new UserService(users);
var user = await userService.AddAsync(new UserPostDto { Name = "Aluno", Email = "a@example.com", Password = "senha-teste" });
Check(user.Role == "Admin", "Primeiro usuário recebe perfil Admin");
var student = await userService.AddAsync(new UserPostDto { Name = "Segundo", Email = "student@example.com", Password = "senha-teste" });
Check(student.Role == "Student", "Usuários seguintes recebem perfil Student");
var salt = users.Items.Single(item => item.Id == user.Id).PasswordSalt!.ToArray();
Check(users.Items.Single(item => item.Id == user.Id).PasswordHash!.Length == 64, "Hash HMACSHA512 gerado");
var updatedUser = await userService.UpdateAsync(user.Id, new UserPutDto { Name = "Novo", Email = "b@example.com", Password = "outra-senha" });
Check(updatedUser!.Id == user.Id && updatedUser.Name == "Novo", "ID de usuário recebido separadamente");
Check(salt.SequenceEqual(users.Items.Single(item => item.Id == user.Id).PasswordSalt!), "Atualização de nome e e-mail preserva salt");
await ExpectNotFound(async () => await userService.UpdateAsync(999, new UserPutDto { Name = "Ausente", Email = "x@example.com", Password = "senha-teste" }));
var enrollments = new FakeEnrollmentRepository();
enrollments.Items.Add(new Enrollment { Id = 1, SchoolClassId = schoolClass.Id });
var grades = new FakeGradeRepository();
var gradeService = new GradeService(grades, enrollments, schoolClasses);
var grade = await gradeService.AddAsync(new GradePostDto { EnrollmentId = 1, GradeValue = 80 });
Check(grade.EnrollmentId == 1 && grade.GradeValue == 80 && grade.IsApproved && grade.DateGrade != default, "Mapeamento de criação da nota");
var changed = await gradeService.UpdateAsync(new GradePutDto { Id = grade.Id, EnrollmentId = 1, GradeValue = 90 });
Check(changed!.GradeValue == 90 && grades.Items.Single().CourseId == course.Id, "Atualização resolve o curso pela SchoolClass");
var enrollmentService = new EnrollmentService(enrollments);
foreach (var operation in new Func<Task>[] {
    async () => await courseService.GetByIdAsync(999),
    async () => await schoolClassService.GetByIdAsync(999),
    async () => await userService.GetByIdAsync(999),
    async () => await gradeService.GetByIdAsync(999),
    async () => await enrollmentService.GetByIdAsync(999),
    async () => await courseService.DeleteAsync(999),
    async () => await schoolClassService.DeleteAsync(999),
    async () => await userService.DeleteAsync(999),
    async () => await gradeService.DeleteAsync(999),
    async () => await enrollmentService.DeleteAsync(999),
    async () => await courseService.UpdateAsync(new CoursePutDto { Id = 999 }),
    async () => await enrollmentService.UpdateAsync(new EnrollmentPutDto { Id = 999 }),
    async () => await schoolClassService.UpdateAsync(new SchoolClassPutDto { Id = 999 }),
    async () => await gradeService.UpdateAsync(new GradePutDto { Id = 999, EnrollmentId = 1 }),
    async () => await schoolClassService.AddAsync(new SchoolClassPostDto { CourseId = 999 }),
    async () => await gradeService.UpdateAsync(new GradePutDto { Id = grade.Id, EnrollmentId = 999 })
}) await ExpectNotFound(operation);
await courseService.DeleteAsync(course.Id);
Check((await courseService.GetAllAsync()).Count == 1, "Serviço devolve registros fornecidos pelo repositório");
Console.WriteLine("Todas as verificações de serviços passaram.");

static void Check(bool value, string message)
{
    if (!value) throw new Exception(message);
    Console.WriteLine($"OK: {message}");
}

static async Task ExpectNotFound(Func<Task> operation)
{
    try { await operation(); }
    catch (NotFoundException) { Console.WriteLine("OK: Recurso ausente gera NotFoundException"); return; }
    throw new Exception("Era esperada NotFoundException para recurso ausente.");
}
