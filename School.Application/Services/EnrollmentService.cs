using School.Application.DTO.Enrollment;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IClassRepository _classRepository;

    public EnrollmentService(IEnrollmentRepository repository, IUserRepository userRepository, IClassRepository classRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
        ArgumentNullException.ThrowIfNull(classRepository);
        _classRepository = classRepository;
    }

    public async Task<EnrollmentGetDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        return entity is null || entity.Deleted ? null : ToDto(entity);
    }

    public async Task<List<EnrollmentGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Where(entity => !entity.Deleted).Select(ToDto).ToList();
    }

    public async Task<EnrollmentGetDto> AddAsync(EnrollmentPostDto dto)
    {
        ServiceValidation.Validate(dto);
        await ValidateRelationsAsync(dto.UserId, dto.ClassId, dto.EnrollmentDate, dto.ExpirationDate);
        var entity = new Enrollment();
        entity.UserId = dto.UserId;
        entity.ClassId = dto.ClassId;
        entity.EnrollmentDate = dto.EnrollmentDate;
        entity.ExpirationDate = dto.ExpirationDate;
        entity.IsActive = dto.IsActive;
        return ToDto(await _repository.AddAsync(entity));
    }

    public async Task<EnrollmentGetDto?> UpdateAsync(EnrollmentPutDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null || entity.Deleted) return null;
        await ValidateRelationsAsync(dto.UserId, dto.ClassId, dto.EnrollmentDate, dto.ExpirationDate);
        entity.UserId = dto.UserId;
        entity.ClassId = dto.ClassId;
        entity.EnrollmentDate = dto.EnrollmentDate;
        entity.ExpirationDate = dto.ExpirationDate;
        entity.IsActive = dto.IsActive;
        return ToDto(await _repository.UpdateAsync(entity));
    }

    public async Task<EnrollmentGetDto?> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null || entity.Deleted) return null;
        var deleted = await _repository.DeleteAsync(id);
        return deleted is null ? null : ToDto(deleted);
    }

    private async Task ValidateRelationsAsync(int userId, int classId, DateTime enrollmentDate, DateTime expirationDate)
    {
        ServiceValidation.Require(enrollmentDate != default && expirationDate > enrollmentDate, "A expiração deve ser posterior à matrícula.");
        ServiceValidation.Require(enrollmentDate.Kind == DateTimeKind.Utc && expirationDate.Kind == DateTimeKind.Utc, "Informe as datas em UTC.");
        var user = await _userRepository.GetByIdAsync(userId);
        var schoolClass = await _classRepository.GetByIdAsync(classId);
        ServiceValidation.Require(user is not null && !user.Deleted, "Usuário inexistente ou excluído.");
        ServiceValidation.Require(schoolClass is not null && !schoolClass.Deleted, "Turma inexistente ou excluída.");
    }

    private static EnrollmentGetDto ToDto(Enrollment entity) => new()
    {
        Id = entity.Id,
        UserId = entity.UserId,
        ClassId = entity.ClassId,
        EnrollmentDate = entity.EnrollmentDate,
        ExpirationDate = entity.ExpirationDate,
        IsActive = entity.IsActive
    };
}
