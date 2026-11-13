using School.Application.DTO.SchoolClass;
using School.Application.DTO.Enrollment;
using School.Application.DTO.User;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }
    public async Task<EnrollmentGetDetailDto?> GetByIdAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
        {
            return null;
        }

        return new EnrollmentGetDetailDto
        {
            Id = enrollment.Id,
            EnrollmentDate = enrollment.EnrollmentDate,
            EnrollmentExpiration = enrollment.ExpirationDate,
            IsActive = enrollment.IsActive,
            User = new UserGetDto
            {
                Id = enrollment.User.Id,
                Name = enrollment.User.Name,
                Email = enrollment.User.Email,
            },
            SchoolClass = new SchoolClassGetDto
            {
                Id = enrollment.SchoolClass.Id,
                Name = enrollment.SchoolClass.Name,
                Description = enrollment.SchoolClass.Description,
            }
        };
    }

    public async Task<List<EnrollmentGetDetailDto>> GetAllAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllAsync();
        var enrollmentsGetDetailsDto = new List<EnrollmentGetDetailDto>();
        enrollmentsGetDetailsDto.AddRange(enrollments.Select(enrollment => new EnrollmentGetDetailDto
        {
            Id = enrollment.Id,
            EnrollmentDate = enrollment.EnrollmentDate,
            EnrollmentExpiration = enrollment.ExpirationDate,
            IsActive = enrollment.IsActive,
            User = new UserGetDto
            {
                Id = enrollment.UserId,
                Name = enrollment.User.Name,
                Email = enrollment.User.Email,
            },
            SchoolClass = new SchoolClassGetDto
            {
                Id = enrollment.SchoolClass.Id,
                Name = enrollment.SchoolClass.Name,
                Description = enrollment.SchoolClass.Description,
            }
        }));
        return enrollmentsGetDetailsDto;
    }

    public async Task<EnrollmentGetDto> AddAsync(EnrollmentPostDto dto)
    {
        var enrollment = new Enrollment
        {
            UserId = dto.UserId,
            SchoolClassId = dto.SchoolClassId,
            EnrollmentDate = DateTime.UtcNow,
            ExpirationDate =  dto.ExpirationDate,
            IsActive = true
        };

        var createdMatricula = await _enrollmentRepository.AddAsync(enrollment);
        return new EnrollmentGetDto
        {
            Id = createdMatricula.Id,
            UserId = createdMatricula.UserId,
            SchoolClassId = createdMatricula.SchoolClassId,
            EnrollmentDate = createdMatricula.EnrollmentDate,
            ExpirationDate = createdMatricula.ExpirationDate,
            IsActive = createdMatricula.IsActive
        };
    }

    public async Task<EnrollmentGetDto?> UpdateAsync(EnrollmentPutDto dto)
    {
        var enrollment = new Enrollment
        {
            Id = dto.Id,
            SchoolClassId = dto.SchoolClassId,
            ExpirationDate = dto.ExpirationDate,
        };
        var updatedEnrollment = await _enrollmentRepository.UpdateAsync(enrollment);
        if (updatedEnrollment == null) return null;
        return new EnrollmentGetDto
        {
            Id = updatedEnrollment.Id,
            UserId = updatedEnrollment.UserId,
            SchoolClassId = updatedEnrollment.SchoolClassId,
            EnrollmentDate = updatedEnrollment.EnrollmentDate,
            ExpirationDate = updatedEnrollment.ExpirationDate,
            IsActive = updatedEnrollment.IsActive
        };
    }

    public async Task<EnrollmentGetDto?> DeleteAsync(int id)
    {
        var deletedEnrollment = await _enrollmentRepository.DeleteAsync(id);
        if (deletedEnrollment == null)
        {
            return null;
        }

        return new EnrollmentGetDto
        {
            Id = deletedEnrollment.Id,
            UserId = deletedEnrollment.UserId,
            SchoolClassId = deletedEnrollment.SchoolClassId,
            EnrollmentDate = deletedEnrollment.EnrollmentDate,
            ExpirationDate = deletedEnrollment.ExpirationDate,
            IsActive = deletedEnrollment.IsActive
        };
    }
}
