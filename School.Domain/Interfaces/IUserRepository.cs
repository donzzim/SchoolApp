using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<User?> DeleteAsync(int id);
}
