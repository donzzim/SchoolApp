using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Infra.Data.Repositories;

public class GradeRepository : IGradeRepository
{
    public Task<Grade> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Grade>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Grade> AddAsync(Grade grade)
    {
        throw new NotImplementedException();
    }

    public Task<Grade> UpdateAsync(Grade grade)
    {
        throw new NotImplementedException();
    }

    public Task<Grade> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}