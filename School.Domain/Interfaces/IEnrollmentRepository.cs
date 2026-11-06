using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface IEnrollmentRepository
{
    Task<Enrollment> GetByIdAsync(int id);
    Task<List<Enrollment>> GetAllAsync();
    Task<Enrollment> AddAsync(Enrollment enrollment);
    Task<Enrollment> UpdateAsync(Enrollment enrollment);
    Task<Enrollment> DeleteAsync(int id);
}