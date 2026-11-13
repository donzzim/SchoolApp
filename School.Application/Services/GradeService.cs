using School.Application.DTO.Grade;

using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ISchoolClassRepository _schoolClassRepository;
    public GradeService(IGradeRepository gradeRepository, IEnrollmentRepository enrollmentRepository, ISchoolClassRepository schoolClassRepository)
    {
        _gradeRepository = gradeRepository;
        _enrollmentRepository = enrollmentRepository;
        _schoolClassRepository = schoolClassRepository;
    }

    public async Task<GradeGetDto?> GetByIdAsync(int id)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        if (grade == null) return null;
        return new GradeGetDto
        {
            Id = grade.Id,
            EnrollmentId = grade.EnrollmentId,
            GradeValue = grade.GradeValue,
            DateGrade = grade.DateGrade,
            IsApproved = grade.IsApproved,
        };
    }

    public async Task<List<GradeGetDto>> GetAllAsync()
    {
        var entities = await _gradeRepository.GetAllAsync();
        var result = new List<GradeGetDto>();
        result.AddRange(entities.Select(grade => new GradeGetDto
        {
            Id = grade.Id,
            EnrollmentId = grade.EnrollmentId,
            GradeValue = grade.GradeValue,
            DateGrade = grade.DateGrade,
            IsApproved = grade.IsApproved,
        }));
        return result;
    }

    public async Task<GradeGetDto> AddAsync(GradePostDto dto)
    {
        var grade = new Grade
        {
            EnrollmentId = dto.EnrollmentId,
            GradeValue = dto.GradeValue,
            IsApproved = dto.GradeValue >= 60,
            DateGrade = DateTime.Now
        };

        var createdGrade = await _gradeRepository.AddAsync(grade);

        return new GradeGetDto
        {
            Id = createdGrade.Id,
            EnrollmentId = createdGrade.EnrollmentId,
            GradeValue = createdGrade.GradeValue,
            DateGrade = createdGrade.DateGrade,
            IsApproved = createdGrade.IsApproved,
        };
    }

    public async Task<GradeGetDto?> UpdateAsync(GradePutDto dto)
    {
        var courseId = await GetCourseIdAsync(dto.EnrollmentId);
        var grade = new Grade
        {
            Id = dto.Id, EnrollmentId = dto.EnrollmentId, CourseId = courseId, GradeValue = dto.GradeValue
        };

        var saved = await _gradeRepository.UpdateAsync(grade);
        if (saved == null) return null;
        return new GradeGetDto
        {
            Id = saved.Id,
            EnrollmentId = saved.EnrollmentId,
            GradeValue = saved.GradeValue,
            DateGrade = saved.DateGrade,
            IsApproved = saved.IsApproved,
        };
    }

    public async Task<GradeGetDto?> DeleteAsync(int id)
    {
        var deleted = await _gradeRepository.DeleteAsync(id);
        if (deleted == null) return null;
        return new GradeGetDto
        {
            Id = deleted.Id,
            EnrollmentId = deleted.EnrollmentId,
            GradeValue = deleted.GradeValue,
            DateGrade = deleted.DateGrade,
            IsApproved = deleted.IsApproved,
        };
    }
    private async Task<int> GetCourseIdAsync(int enrollmentId)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId)
            ?? throw new InvalidOperationException("Matrícula inexistente.");
        var schoolClass = await _schoolClassRepository.GetByIdAsync(enrollment.SchoolClassId)
            ?? throw new InvalidOperationException("Turma inexistente.");
        return schoolClass.CourseId;
    }

}
