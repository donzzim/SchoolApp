using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class SchoolClassRepository : ISchoolClassRepository
{
    private readonly ApplicationDbContext _context;

    public SchoolClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SchoolClass?> GetByIdAsync(int id)
    {
        return await _context.SchoolClasses.Include(c => c.Course).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<SchoolClass>> GetAllAsync()
    {
        return await _context.SchoolClasses.Include(x => x.Course).Where((x => x.Deleted == false)).ToListAsync();
    }

    public async Task<SchoolClass> AddAsync(SchoolClass c)
    {
        _context.SchoolClasses.Add(c);
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<SchoolClass?> UpdateAsync(SchoolClass c)
    {
        var existing = await _context.SchoolClasses.FindAsync(c.Id);
        if (existing == null) return null;
        existing.Name = c.Name;
        existing.Description = c.Description;
        existing.CourseId = c.CourseId;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<SchoolClass?> DeleteAsync(int id)
    {
        var entity = await _context.SchoolClasses.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Deleted = true;
        _context.SchoolClasses.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
