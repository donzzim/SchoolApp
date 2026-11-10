using School.Application.DTO.Course;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    public CourseService(ICourseRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<CourseGetDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        return entity is null || entity.Deleted ? null : ToDto(entity);
    }

    public async Task<List<CourseGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Where(entity => !entity.Deleted).Select(ToDto).ToList();
    }

    public async Task<CourseGetDto> AddAsync(CoursePostDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = new Course();
        entity.Name = dto.Name!.Trim();
        entity.Description = dto.Description!.Trim();
        return ToDto(await _repository.AddAsync(entity));
    }

    public async Task<CourseGetDto?> UpdateAsync(CoursePutDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null || entity.Deleted) return null;
        entity.Name = dto.Name!.Trim();
        entity.Description = dto.Description!.Trim();
        return ToDto(await _repository.UpdateAsync(entity));
    }

    public async Task<CourseGetDto?> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null || entity.Deleted) return null;
        var deleted = await _repository.DeleteAsync(id);
        return deleted is null ? null : ToDto(deleted);
    }

    private static CourseGetDto ToDto(Course entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description
    };
}
