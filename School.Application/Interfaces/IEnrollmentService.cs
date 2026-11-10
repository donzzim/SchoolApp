using School.Application.DTO.Enrollment;

namespace School.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentGetDto?> GetByIdAsync(int id);
    Task<List<EnrollmentGetDto>> GetAllAsync();
    Task<EnrollmentGetDto> AddAsync(EnrollmentPostDto dto);
    Task<EnrollmentGetDto?> UpdateAsync(EnrollmentPutDto dto);
    Task<EnrollmentGetDto?> DeleteAsync(int id);
}
