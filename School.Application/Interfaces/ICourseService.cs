using School.Application.DTO.Course;

namespace School.Application.Interfaces;

public interface ICourseService
{
    Task<CourseGetDto?> GetByIdAsync(int id);
    Task<List<CourseGetDto>> GetAllAsync();
    Task<CourseGetDto> AddAsync(CoursePostDto dto);
    Task<CourseGetDto?> UpdateAsync(CoursePutDto dto);
    Task<CourseGetDto?> DeleteAsync(int id);
}
