using School.Application.DTO.Course;

using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;

    }

    public async Task<CourseGetDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null) return null;
        return new CourseGetDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
        };
    }

    public async Task<List<CourseGetDto>> GetAllAsync()
    {
        var entities = await _courseRepository.GetAllAsync();
        var result = new List<CourseGetDto>();
        result.AddRange(entities.Select(course => new CourseGetDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
        }));
        return result;
    }

    public async Task<CourseGetDto> AddAsync(CoursePostDto dto)
    {

        var course = new Course
        {
            Name = dto.Name, Description = dto.Description
        };

        var saved = await _courseRepository.AddAsync(course);

        return new CourseGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
        };
    }

    public async Task<CourseGetDto?> UpdateAsync(CoursePutDto dto)
    {

        var course = new Course
        {
            Id = dto.Id, Name = dto.Name, Description = dto.Description
        };

        var saved = await _courseRepository.UpdateAsync(course);
        if (saved == null) return null;
        return new CourseGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
        };
    }

    public async Task<CourseGetDto?> DeleteAsync(int id)
    {
        var deleted = await _courseRepository.DeleteAsync(id);
        if (deleted == null) return null;
        return new CourseGetDto
        {
            Id = deleted.Id,
            Name = deleted.Name,
            Description = deleted.Description,
        };
    }


}
