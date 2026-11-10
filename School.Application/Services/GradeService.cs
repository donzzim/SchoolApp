using School.Application.DTO.Grade;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _repository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IClassRepository _classRepository;

    public GradeService(IGradeRepository repository, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository, IClassRepository classRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        ArgumentNullException.ThrowIfNull(courseRepository);
        _courseRepository = courseRepository;
        ArgumentNullException.ThrowIfNull(enrollmentRepository);
        _enrollmentRepository = enrollmentRepository;
        ArgumentNullException.ThrowIfNull(classRepository);
        _classRepository = classRepository;
    }

    public async Task<GradeGetDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        return entity is null || entity.Deleted ? null : ToDto(entity);
    }

    public async Task<List<GradeGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Where(entity => !entity.Deleted).Select(ToDto).ToList();
    }

    public async Task<GradeGetDto> AddAsync(GradePostDto dto)
    {
        ServiceValidation.Validate(dto);
        await ValidateRelationsAsync(dto.CourseId, dto.EnrollmentId, dto.DateGrade);
        var entity = new Grade();
        entity.CourseId = dto.CourseId;
        entity.EnrollmentId = dto.EnrollmentId;
        entity.GradeValue = dto.GradeValue;
        entity.DateGrade = dto.DateGrade;
        entity.IsApproved = dto.IsApproved;
        return ToDto(await _repository.AddAsync(entity));
    }

    public async Task<GradeGetDto?> UpdateAsync(GradePutDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null || entity.Deleted) return null;
        await ValidateRelationsAsync(dto.CourseId, dto.EnrollmentId, dto.DateGrade);
        entity.CourseId = dto.CourseId;
        entity.EnrollmentId = dto.EnrollmentId;
        entity.GradeValue = dto.GradeValue;
        entity.DateGrade = dto.DateGrade;
        entity.IsApproved = dto.IsApproved;
        return ToDto(await _repository.UpdateAsync(entity));
    }

    public async Task<GradeGetDto?> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null || entity.Deleted) return null;
        var deleted = await _repository.DeleteAsync(id);
        return deleted is null ? null : ToDto(deleted);
    }

    private async Task ValidateRelationsAsync(int courseId, int enrollmentId, DateTime dateGrade)
    {
        ServiceValidation.Require(dateGrade != default && dateGrade.Kind == DateTimeKind.Utc, "Informe uma data de avaliação em UTC.");
        var course = await _courseRepository.GetByIdAsync(courseId);
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
        ServiceValidation.Require(course is not null && !course.Deleted, "Curso inexistente ou excluído.");
        ServiceValidation.Require(enrollment is not null && !enrollment.Deleted, "Matrícula inexistente ou excluída.");
        var schoolClass = await _classRepository.GetByIdAsync(enrollment!.ClassId);
        ServiceValidation.Require(schoolClass is not null && !schoolClass.Deleted && schoolClass.CourseId == courseId, "A nota deve pertencer ao curso da turma da matrícula.");
    }

    private static GradeGetDto ToDto(Grade entity) => new()
    {
        Id = entity.Id,
        CourseId = entity.CourseId,
        EnrollmentId = entity.EnrollmentId,
        GradeValue = entity.GradeValue,
        DateGrade = entity.DateGrade,
        IsApproved = entity.IsApproved
    };
}
