using School.Application.DTO.User;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<UserGetDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        return entity is null || entity.Deleted ? null : ToDto(entity);
    }

    public async Task<List<UserGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Where(entity => !entity.Deleted).Select(ToDto).ToList();
    }

    public async Task<UserGetDto> AddAsync(UserPostDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = new User();
        entity.Name = dto.Name!.Trim();
        entity.Email = dto.Email!.Trim();
        entity.Profile = dto.Profile!.Trim();
        if (dto.Password is not null) SetPassword(entity, dto.Password);
        return ToDto(await _repository.AddAsync(entity));
    }

    public async Task<UserGetDto?> UpdateAsync(UserPutDto dto)
    {
        ServiceValidation.Validate(dto);
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null || entity.Deleted) return null;
        entity.Name = dto.Name!.Trim();
        entity.Email = dto.Email!.Trim();
        entity.Profile = dto.Profile!.Trim();
        if (dto.Password is not null) SetPassword(entity, dto.Password);
        return ToDto(await _repository.UpdateAsync(entity));
    }

    public async Task<UserGetDto?> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null || entity.Deleted) return null;
        var deleted = await _repository.DeleteAsync(id);
        return deleted is null ? null : ToDto(deleted);
    }

    private static void SetPassword(User user, string password)
    {
        user.PasswordSalt = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
        user.PasswordHash = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(
            password, user.PasswordSalt, 600_000, System.Security.Cryptography.HashAlgorithmName.SHA256, 32);
    }

    private static UserGetDto ToDto(User entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Email = entity.Email,
        Profile = entity.Profile
    };
}
