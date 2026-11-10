using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _context;

    public ClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Class?> GetByIdAsync(int id)
    {
        return await _context.Classes.FindAsync(id);
    }

    public async Task<List<Class>> GetAllAsync()
    {
        return await _context.Classes.ToListAsync();
    }

    public async Task<Class> AddAsync(Class c)
    {
        _context.Classes.Add(c);
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<Class> UpdateAsync(Class c)
    {
        _context.Classes.Update(c);
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<Class?> DeleteAsync(int id)
    {
        var entity = await _context.Classes.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Deleted = true;
        _context.Classes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
