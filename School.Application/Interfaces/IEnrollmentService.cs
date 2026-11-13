using School.Application.DTO.Enrollment;

namespace School.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentGetDetailDto?> GetByIdAsync(int id);
    Task<List<EnrollmentGetDetailDto>> GetAllAsync();
    Task<EnrollmentGetDto> AddAsync(EnrollmentPostDto dto);
    Task<EnrollmentGetDto?> UpdateAsync(EnrollmentPutDto dto);
    Task<EnrollmentGetDto?> DeleteAsync(int id);
}
