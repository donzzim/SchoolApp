using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface IGradeRepository
{
    Task<Grade?> GetByIdAsync(int id);
    Task<List<Grade>> GetAllAsync();
    Task<Grade> AddAsync(Grade grade);
    Task<Grade> UpdateAsync(Grade grade);
    Task<Grade?> DeleteAsync(int id);
}