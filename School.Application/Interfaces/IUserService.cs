using School.Application.DTO.User;

namespace School.Application.Interfaces;

public interface IUserService
{
    Task<UserGetDto?> GetByIdAsync(int id);
    Task<List<UserGetDto>> GetAllAsync();
    Task<UserGetDto> AddAsync(UserPostDto dto);
    Task<UserGetDto?> UpdateAsync(UserPutDto dto);
    Task<UserGetDto?> DeleteAsync(int id);
}
