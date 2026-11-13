using System.Security.Cryptography;
using System.Text;
using School.Application.DTO.User;

using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;

    }

    public async Task<UserGetDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return new UserGetDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public async Task<List<UserGetDto>> GetAllAsync()
    {
        var entities = await _userRepository.GetAllAsync();
        var result = new List<UserGetDto>();
        result.AddRange(entities.Select(user => new UserGetDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        }));
        return result;
    }

    public async Task<UserGetDto> AddAsync(UserPostDto dto)
    {
        using var hmac = new HMACSHA512();
        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        byte[] passwordSalt = hmac.Key;

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Deleted = false,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Profile = "Student"
        };
        var saved = await _userRepository.AddAsync(user);

        return new UserGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
        };
    }

    public async Task<UserGetDto?> UpdateAsync(int userId, UserPutDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;
        user.Name = dto.Name;
        user.Email = dto.Email;
        var updatedUser =  await _userRepository.UpdateAsync(user);
        return new UserGetDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,

        };
    }

    public async Task<UserGetDto?> DeleteAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        if (deleted == null) return null;
        return new UserGetDto
        {
            Id = deleted.Id,
            Name = deleted.Name,
            Email = deleted.Email,
        };
    }
}
