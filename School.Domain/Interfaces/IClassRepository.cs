using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface IClassRepository
{
    Task<Class?> GetByIdAsync(int id);
    Task<List<Class>> GetAllAsync();
    Task<Class> AddAsync(Class c);
    Task<Class> UpdateAsync(Class c);
    Task<Class?> DeleteAsync(int id);
}