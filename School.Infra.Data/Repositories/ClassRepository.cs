using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Infra.Data.Repositories;

public class ClassRepository : IClassRepository
{
    public Task<Class> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Class>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Class> AddAsync(Class c)
    {
        throw new NotImplementedException();
    }

    public Task<Class> UpdateAsync(Class c)
    {
        throw new NotImplementedException();
    }

    public Task<Class> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}