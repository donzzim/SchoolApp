using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Infra.Data.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    public Task<Enrollment> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Enrollment>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Enrollment> AddAsync(Enrollment enrollment)
    {
        throw new NotImplementedException();
    }

    public Task<Enrollment> UpdateAsync(Enrollment enrollment)
    {
        throw new NotImplementedException();
    }

    public Task<Enrollment> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}