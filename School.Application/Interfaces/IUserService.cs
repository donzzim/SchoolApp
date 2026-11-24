using School.Application.Exceptions;
using School.Application.DTO.User;

namespace School.Application.Interfaces;

public interface IUserService
{
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<UserGetDto?> GetByIdAsync(int id);
    Task<List<UserGetDto>> GetAllAsync();
    Task<UserGetDto> AddAsync(UserPostDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<UserGetDto?> UpdateAsync(int userId, UserPutDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<UserGetDto?> DeleteAsync(int id);
    Task<bool> UserExistsAsync();
}
