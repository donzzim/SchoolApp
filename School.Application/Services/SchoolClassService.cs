using School.Application.DTO.SchoolClass;
using School.Application.DTO.Course;
using School.Application.Exceptions;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class SchoolClassService : ISchoolClassService
{
    private readonly ISchoolClassRepository _schoolClassRepository;
    private readonly ICourseRepository _courseRepository;

    public SchoolClassService(ISchoolClassRepository schoolClassRepository,  ICourseRepository courseRepository)
    {
        _schoolClassRepository = schoolClassRepository;
        _courseRepository = courseRepository;
    }

    public async Task<SchoolClassGetDetailDto?> GetByIdAsync(int id)
    {
        var schoolClass = await _schoolClassRepository.GetByIdAsync(id);
        if (schoolClass == null) throw new NotFoundException("School Class not found");
        return new SchoolClassGetDetailDto
        {
            Id = schoolClass.Id,
            Name = schoolClass.Name,
            Description = schoolClass.Description,
            Course = schoolClass.Course is null ? null : new CourseGetDto
            {
                Id = schoolClass.Course.Id,
                Name = schoolClass.Course.Name,
                Description = schoolClass.Course.Description
            }
        };
    }

    public async Task<List<SchoolClassGetDto>> GetAllAsync()
    {
        var entities = await _schoolClassRepository.GetAllAsync();
        var result = new List<SchoolClassGetDto>();
        result.AddRange(entities.Select(schoolClass => new SchoolClassGetDto
        {
            Id = schoolClass.Id,
            Name = schoolClass.Name,
            Description = schoolClass.Description,
            CourseId = schoolClass.CourseId,
        }));
        return result;
    }

    public async Task<SchoolClassGetDto> AddAsync(SchoolClassPostDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(dto.CourseId);
        if (course == null) throw new NotFoundException("Course not found");

        var schoolClass = new SchoolClass
        {
            Name = dto.Name, Description = dto.Description, CourseId = dto.CourseId
        };

        var saved = await _schoolClassRepository.AddAsync(schoolClass);

        return new SchoolClassGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
            CourseId = saved.CourseId,
        };
    }

    public async Task<SchoolClassGetDto?> UpdateAsync(SchoolClassPutDto dto)
    {
        var schoolClass = await _schoolClassRepository.GetByIdAsync(dto.Id);
        if (schoolClass == null) throw new NotFoundException("School Class not found");

        var course = await _courseRepository.GetByIdAsync(dto.CourseId);
        if (course == null) throw new NotFoundException("Course not found");

        schoolClass.Id = dto.Id;
        schoolClass.Name = dto.Name;
        schoolClass.Description = dto.Description;
        schoolClass.CourseId = dto.CourseId;

        var saved = await _schoolClassRepository.UpdateAsync(schoolClass);
        if (saved == null) throw new NotFoundException("School Class not found");
        return new SchoolClassGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
            CourseId = saved.CourseId,
        };
    }

    public async Task<SchoolClassGetDto?> DeleteAsync(int id)
    {
        var deleted = await _schoolClassRepository.DeleteAsync(id);
        if (deleted == null) throw new NotFoundException("School Class not found");
        return new SchoolClassGetDto
        {
            Id = deleted.Id,
            Name = deleted.Name,
            Description = deleted.Description,
            CourseId = deleted.CourseId,
        };
    }


}
