using School.Domain.Entities;
using School.Domain.Interfaces;

namespace School.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}