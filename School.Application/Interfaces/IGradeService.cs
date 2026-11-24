using School.Application.Exceptions;
using School.Application.DTO.Grade;

namespace School.Application.Interfaces;

public interface IGradeService
{
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<GradeGetDto?> GetByIdAsync(int id);
    Task<List<GradeGetDto>> GetAllAsync();
    Task<GradeGetDto> AddAsync(GradePostDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<GradeGetDto?> UpdateAsync(GradePutDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<GradeGetDto?> DeleteAsync(int id);
}
