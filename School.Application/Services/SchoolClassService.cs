using School.Application.DTO.SchoolClass;
using School.Application.DTO.Course;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class SchoolClassService : ISchoolClassService
{
    private readonly ISchoolClassRepository _schoolClassRepository;

    public SchoolClassService(ISchoolClassRepository schoolClassRepository)
    {
        _schoolClassRepository = schoolClassRepository;

    }

    public async Task<SchoolClassGetDetailDto?> GetByIdAsync(int id)
    {
        var schoolClass = await _schoolClassRepository.GetByIdAsync(id);
        if (schoolClass == null) return null;
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

        var schoolClass = new SchoolClass
        {
            Id = dto.Id, Name = dto.Name, Description = dto.Description, CourseId = dto.CourseId
        };

        var saved = await _schoolClassRepository.UpdateAsync(schoolClass);
        if (saved == null) return null;
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
        if (deleted == null) return null;
        return new SchoolClassGetDto
        {
            Id = deleted.Id,
            Name = deleted.Name,
            Description = deleted.Description,
            CourseId = deleted.CourseId,
        };
    }


}
