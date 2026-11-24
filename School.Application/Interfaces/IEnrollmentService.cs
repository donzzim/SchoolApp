using School.Application.Exceptions;
using School.Application.DTO.Enrollment;

namespace School.Application.Interfaces;

public interface IEnrollmentService
{
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<EnrollmentGetDetailDto?> GetByIdAsync(int id);
    Task<List<EnrollmentGetDetailDto>> GetAllAsync();
    Task<EnrollmentGetDto> AddAsync(EnrollmentPostDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<EnrollmentGetDto?> UpdateAsync(EnrollmentPutDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<EnrollmentGetDto?> DeleteAsync(int id);
}
