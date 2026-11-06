using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course> GetByIdAsync(int id);
    Task<List<Course>> GetAllAsync();
    Task<Course> AddAsync(Course course);
    Task<Course> UpdateAsync(Course course);
    Task<Course> DeleteAsync(int id);
}