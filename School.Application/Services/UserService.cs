using School.Application.Exceptions;
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

    public async Task<bool> UserExistsAsync()
    {
        return await _userRepository.UserExistsAsync();
    }

    public async Task<UserGetDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new NotFoundException("User not found");
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

        var existingUser = await _userRepository.UserExistsAsync();

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Deleted = false,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = existingUser ? "Student" : "Admin"
        };
        var saved = await _userRepository.AddAsync(user);

        return new UserGetDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
            Role = saved.Role,
        };
    }

    public async Task<UserGetDto?> UpdateAsync(int userId, UserPutDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");
        user.Name = dto.Name;
        user.Email = dto.Email;
        var updatedUser = await _userRepository.UpdateAsync(user);
        if (updatedUser == null) throw new NotFoundException("User not found");
        return new UserGetDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,
            Role = updatedUser.Role,
        };
    }

    public async Task<UserGetDto?> DeleteAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        if (deleted == null) throw new NotFoundException("User not found");
        return new UserGetDto
        {
            Id = deleted.Id,
            Name = deleted.Name,
            Email = deleted.Email,
            Role =  deleted.Role,
        };
    }
}
