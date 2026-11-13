using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.User.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.User.ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var existing = await _context.User.FindAsync(user.Id);
        if (existing == null) return null;
        existing.Name = user.Name;
        existing.Email = user.Email;
        existing.PasswordHash = user.PasswordHash;
        existing.PasswordSalt = user.PasswordSalt;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<User?> DeleteAsync(int id)
    {
        var entity = await _context.User.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Deleted = true;
        _context.User.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
