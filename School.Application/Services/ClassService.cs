using School.Application.DTO.Class;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class ClassService : IClassService
{
    private readonly IClassRepository _repository;
    private readonly ICourseRepository _courseRepository;

    public ClassService(IClassRepository repository, ICourseRepository courseRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        ArgumentNullException.ThrowIfNull(courseRepository);
        _courseRepository = courseRepository;
    }

    public async Task<ClassGetDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        return entity is null || entity.Deleted ? null : ToDto(entity);
    }

    public async Task<List<ClassGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Where(entity => !entity.Deleted).Select(ToDto).ToList();
    }

    public async Task<ClassGetDto> AddAsync(ClassPostDto dto)
    {
        ServiceValidation.Validate(dto);
        await ValidateRelationsAsync(dto.CourseId);
        var entity = new Class();
        entity.Name = dto.Name!.Trim();
        entity.Description = dto.Description!.Trim();
        entity.CourseId = dto.CourseId;
        return ToDto(await _repository.AddAsync(entity));
    }

    public async Task<ClassGetDto?> UpdateAsync(ClassPutDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null || entity.Deleted) return null;
        await ValidateRelationsAsync(dto.CourseId);
        entity.Name = dto.Name!.Trim();
        entity.Description = dto.Description!.Trim();
        entity.CourseId = dto.CourseId;
        return ToDto(await _repository.UpdateAsync(entity));
    }

    public async Task<ClassGetDto?> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null || entity.Deleted) return null;
        var deleted = await _repository.DeleteAsync(id);
        return deleted is null ? null : ToDto(deleted);
    }

    private async Task ValidateRelationsAsync(int courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        ServiceValidation.Require(course is not null && !course.Deleted, "Curso inexistente ou excluído.");
    }

    private static ClassGetDto ToDto(Class entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        CourseId = entity.CourseId
    };
}
