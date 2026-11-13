using School.Domain.Entities;

namespace School.Domain.Interfaces;

public interface ISchoolClassRepository
{
    Task<SchoolClass?> GetByIdAsync(int id);
    Task<List<SchoolClass>> GetAllAsync();
    Task<SchoolClass> AddAsync(SchoolClass c);
    Task<SchoolClass?> UpdateAsync(SchoolClass c);
    Task<SchoolClass?> DeleteAsync(int id);
}
