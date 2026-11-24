using School.Application.Exceptions;
using School.Application.DTO.Course;

namespace School.Application.Interfaces;

public interface ICourseService
{
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<CourseGetDto?> GetByIdAsync(int id);
    Task<List<CourseGetDto>> GetAllAsync();
    Task<CourseGetDto> AddAsync(CoursePostDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<CourseGetDto?> UpdateAsync(CoursePutDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<CourseGetDto?> DeleteAsync(int id);
}
