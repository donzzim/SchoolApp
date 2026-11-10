using School.Application.DTO.Grade;

namespace School.Application.Interfaces;

public interface IGradeService
{
    Task<GradeGetDto?> GetByIdAsync(int id);
    Task<List<GradeGetDto>> GetAllAsync();
    Task<GradeGetDto> AddAsync(GradePostDto dto);
    Task<GradeGetDto?> UpdateAsync(GradePutDto dto);
    Task<GradeGetDto?> DeleteAsync(int id);
}
